﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System.ComponentModel;

public class TerrainChunk : MonoBehaviour
{
    //A 3D chunk index
    public Vector3Int chunkCoord;
    //A 3D chunk position in world space
    public Vector3 chunkWorldPos;
    //Chunk world scale
    public Vector3Int chunkSize;

    //Mesh components
    MeshRenderer meshRenderer;
    MeshFilter meshFilter;
    MeshCollider meshCollider;
    Mesh mesh;

    //BackgroundWorkers (handles multithreading when generating byte values or mesh data)
    BackgroundWorker meshWorker;
    BackgroundWorker byteArrayWorker;

    //Chunk data container (used to pass chunk information between threads)
    ChunkData chunkData;

    //True if chunk mesh is generated, otherwise false
    public bool generated = false;

    void Awake()
    {
        //Get the mesh components from the chunk game object
        meshRenderer = GetComponent<MeshRenderer>();
        meshFilter = GetComponent<MeshFilter>();
        meshCollider = GetComponent<MeshCollider>();
    }
    /// <summary>
    /// Get the local position of the given world space position relative to this chunk
    /// </summary>
    /// <param name="pos">A 3D point in World Space</param>
    /// <returns>A 3D point in local space (floored to integers)</returns>
    public Vector3Int GetLocalPosition(Vector3 pos)
    {
        return Vector3Int.FloorToInt(pos - this.chunkWorldPos);
    }
    /// <summary>
    /// Get the byte value at the given 3D point
    /// </summary>
    /// <param name="pos">A 3D point in world or local space</param>
    /// <param name="isLocalPoint">Pass true if pos parameter is in local space, otherwise false</param>
    /// <returns>The byte value of the block at the given position</returns>
    public byte GetBlockByteValue(Vector3 pos, bool isLocalPoint)
    {
        //If we are a local point (i.e. 0 <= pos < scale in all directions)
        if (isLocalPoint)
        {
            //Get the byte value at the position
            byte outByte = chunkData.GetByteValue(GetByteArrayIndex(pos, chunkSize));

            //If the chunk has saved data at this position override the byte value generated by the terrain noise
            if (chunkData.savedData != null && chunkData.savedData.HasByte(Vector3Int.FloorToInt(pos)))
            {
                outByte = chunkData.savedData.GetByte(Vector3Int.FloorToInt(pos));
            }
            return outByte;
        }
        else
        {
            //If we are here we are not a local point, convert the world space position to local space
            Vector3Int localPos = GetLocalPosition(pos);

            //Get the byte value a the local space position
            byte outByte = chunkData.GetByteValue(GetByteArrayIndex(localPos, chunkSize));

            //If the chunk has saved data at this position override the byte value generated by the terrain noise
            if(chunkData.savedData != null && chunkData.savedData.HasByte(localPos))
            {
                outByte = chunkData.savedData.GetByte(localPos);
            }
            return outByte;
        }
    }
    /// <summary>
    /// Check to see if any of this chunks worker threads have finished their tasks.
    /// If they have, complete them and dispose of the thread
    /// </summary>
    public void WaitForByteArray()
    {
        //If the chunk has not been generated yet, and the thread to generate byte values using perlin noise has completed then continue into the if
        if (!generated && byteArrayWorker != null && !byteArrayWorker.IsBusy && chunkData != null)
        {
            //If we are here the byte values have finished generating. Start the thread to generate the mesh from these values
            meshWorker = new BackgroundWorker();
            meshWorker.DoWork += new DoWorkEventHandler(GenerateMesh);
            meshWorker.RunWorkerAsync(chunkData);

            //Dispose of the thread that generated the byte values
            byteArrayWorker.Dispose();
            byteArrayWorker = null;
        }
        //If the chunk has not been generated yet, and the thread to generate its mesh has completed then continue into the if
        if (!generated && meshWorker != null && !meshWorker.IsBusy && chunkData != null)
        {
            //Create an mesh setting the vertices and triangle arrays to the arrays generated by the mesh generation thread
            Mesh mesh = new Mesh();
            mesh.vertices = chunkData.vertexArray;
            mesh.triangles = chunkData.triangleArray;

            //Recalculate the mesh normals (I calculate the normals myself so this is redundant but its a habit I do anyway)
            //(Removing this line provides no noticeable performance improvements)
            mesh.RecalculateNormals();

            //Update the current chunk mesh
            UpdateMesh(mesh);
            //Set generated to true as the mesh has been created
            generated = true;

            //Dispose of the thread used to generate the mesh from the byte values
            meshWorker.Dispose();
            meshWorker = null;
        }
    }
    /// <summary>
    /// Clear the chunk of all current data, cancel any running threads, set chunk to be ungenerated
    /// </summary>
    public void ClearChunk()
    {
        if (byteArrayWorker != null)
        {
            byteArrayWorker.CancelAsync();
        }
        if (meshWorker != null)
        {
            meshWorker.CancelAsync();
        }

        generated = false;

        if (chunkData == null) return;
        chunkData.Clear();
    }
    /// <summary>
    /// Begin generation of the chunk mesh.
    /// If chunk had previous data, clear it. Start thread to generate byte values at each block from octave noise.
    /// </summary>
    /// <param name="chunkCoord">A 3D chunk index</param>
    /// <param name="chunkWorldPos">A 3D chunk position in world space</param>
    /// <param name="settings">A NoiseSettings struct containing the values to use for the octave noise</param>
    /// <param name="savedData">The saved data for this chunk</param>
    public void GenerateChunk(Vector3Int chunkCoord, Vector3 chunkWorldPos, NoiseSettings settings, SavedChunkData savedData)
    {
        ClearChunk();

        byteArrayWorker = new BackgroundWorker();
        byteArrayWorker.DoWork += new DoWorkEventHandler(GenerateByteArray);

        this.chunkCoord = chunkCoord;
        this.chunkWorldPos = chunkWorldPos;
        chunkData = new ChunkData(chunkWorldPos, chunkSize, settings, chunkSize.x * chunkSize.y * chunkSize.z, savedData);

        //byteArrayThread.Start(chunkData);
        byteArrayWorker.RunWorkerAsync(chunkData);
    }
    /// <summary>
    /// Generate the byte values for a chunk.
    /// </summary>
    /// <param name="sender">The BackgroundWorker invoking this method</param>
    /// <param name="e">Parameter to share ChunkData object information between threads</param>
    public static void GenerateByteArray(object sender, DoWorkEventArgs e)
    {
        //Get the chunk data from the worker argument passed on the method call.
        ChunkData chunkData = (ChunkData)e.Argument;

        //Loop over all local positions (in the x,y, and z directions)
        for (int i = 0; i < chunkData.chunkSize.x; i++)
        {
            for(int j = 0; j < chunkData.chunkSize.y; j++)
            {
                for (int k = 0; k < chunkData.chunkSize.z; k++)
                {
                    Vector3 pos = new Vector3(i, j, k);
                    //Convert the 3D local space coordinate to an index to access a 1D array of byte values
                    int index = GetByteArrayIndex(pos, chunkData.chunkSize);
                    //Generate the byte value at the world space position of the block, with current noise settings
                    //Set the byte value at the index, to the generated value
                    chunkData.SetByteValue(index, GenerateBlock(chunkData.chunkWorldPos + pos, chunkData.settings));

                    //Check if CancelASync has been called on this thread.
                    //If it has stop execution to kill the thread and save resources
                    if (((BackgroundWorker)sender).CancellationPending)
                    {
                        return;
                    }
                }
            }
        }
    }
    /// <summary>
    /// Generate the chunk mesh from the generate byte values
    /// </summary>
    /// <param name="sender">The BackgroundWorker invoking this method</param>
    /// <param name="e">Parameter to share ChunkData object information between threads</param>
    static void GenerateMesh(object sender, DoWorkEventArgs e)
    {
        //Get the chunk data from the worker argument passed on the method call.
        ChunkData chunkData = (ChunkData)e.Argument;

        //Loop over all local space positions
        for (int i = 0; i < chunkData.chunkSize.x; i++)
        {
            for (int k = 0; k < chunkData.chunkSize.z; k++)
            {
                for (int j = 0; j < chunkData.chunkSize.y; j++)
                {
                    Vector3 pos = new Vector3(i, j, k);
                    //Loop over each direction
                    foreach(Vector3 dir in CustomMath.directions)
                    {
                        //Get the block adjacent to the current block in the given direction
                        Vector3 surrounding = pos + dir;
                        //Get the calculate adjacent vector as an integer vector
                        Vector3Int surroundingInt = Vector3Int.FloorToInt(surrounding);

                        //If the chunk has a solid block at the current position continue into the if (as we will need to be the cube faces around it)
                        //OR If the chunk has saved data at this position and it is a solid block continue into this if
                        if(chunkData.byteArr[GetByteArrayIndex(pos, chunkData.chunkSize)] == 1 || (chunkData.savedData != null && chunkData.savedData.HasByte(Vector3Int.FloorToInt(pos)) && chunkData.savedData.GetByte(Vector3Int.FloorToInt(pos)) == 1))
                        {
                            //Because of short circuit we could have entered this if and not checked if the block was actually destroyed by a player
                            //Check if current block is actually an air block, if so continue to the next block
                            if(chunkData.savedData != null && chunkData.savedData.HasByte(Vector3Int.FloorToInt(pos)) && chunkData.savedData.GetByte(Vector3Int.FloorToInt(pos)) == 0)
                            {
                                continue;
                            }
                            //If the calculated adjacent block is inside the bounds of this chunk continue into this if
                            if (surrounding.x < chunkData.chunkSize.x && surrounding.x >= 0 && surrounding.y < chunkData.chunkSize.y && surrounding.y >= 0 && surrounding.z < chunkData.chunkSize.z && surrounding.z >= 0)
                            {
                                //If the block adjacent to the current block is air then construct a cube face in its direction
                                if (chunkData.byteArr[GetByteArrayIndex(surrounding, chunkData.chunkSize)] == 0)
                                {
                                    //Array of directions for constructing the cube face with quad normal = dir
                                    Vector3[] wlDir = CustomMath.directionDictionary[dir];
                                    //Add a quad to the current vertices and triangles of the chunk, with width and length relative to the quad normal
                                    AddQuad(chunkData.vertices, chunkData.triangles, pos + wlDir[2], wlDir[0], wlDir[1], dir);
                                }
                                //Else if the chunk has saved data at this adjacent block and its saved data is an air block construct a cube face in its direction
                                else if((chunkData.savedData != null && chunkData.savedData.HasByte(surroundingInt) && chunkData.savedData.GetByte(surroundingInt) == 0))
                                {
                                    //Array of directions for constructing the cube face with quad normal = dir
                                    Vector3[] wlDir = CustomMath.directionDictionary[dir];
                                    //Add a quad to the current vertices and triangles of the chunk, with width and length relative to the quad normal
                                    AddQuad(chunkData.vertices, chunkData.triangles, pos + wlDir[2], wlDir[0], wlDir[1], dir);
                                }
                            }
                            //Else if the block is outside the current chunk bounds quickly generate the value the block would have in the next chunk
                            //Or if the chunk has saved data about the adjacent block outside the bounds and that block is air construct a face in its direction
                            //NOTE: the chunk saved data will contain information about blocks outside of the chunk bounds due to how saved information is added to the chunks
                            else if(GenerateBlock(chunkData.chunkWorldPos + surrounding, chunkData.settings) == 0 || (chunkData.savedData != null && chunkData.savedData.HasByte(surroundingInt) && chunkData.savedData.GetByte(surroundingInt) == 0))
                            {
                                //Array of directions for constructing the cube face with quad normal = dir
                                Vector3[] wlDir = CustomMath.directionDictionary[dir];
                                //Add a quad to the current vertices and triangles of the chunk, with width and length relative to the quad normal
                                AddQuad(chunkData.vertices, chunkData.triangles, pos + wlDir[2], wlDir[0], wlDir[1], dir);
                            }
                        }

                        //Check if CancelASync has been called on this thread.
                        //If it has stop execution to kill the thread and save resources
                        if (((BackgroundWorker)sender).CancellationPending)
                        {
                            return;
                        }
                    }
                }
            }
        }

        //Convert the vertex and triangle list to arrays (as Mesh object only takes arrays)
        //This is done in the separate thread to offload more work from the main thread
        chunkData.vertexArray = chunkData.vertices.ToArray();
        chunkData.triangleArray = chunkData.triangles.ToArray();
    }
    /// <summary>
    /// Set the chunk mesh filter and collider to use the given mesh
    /// </summary>
    /// <param name="mesh">A completed chunk mesh</param>
    void UpdateMesh(Mesh mesh)
    {
        this.mesh = mesh;
        meshFilter.sharedMesh = mesh;
        meshCollider.sharedMesh = mesh;
    }
    /// <summary>
    /// Adds vertices and triangle indices for a quad to the given vertex/triangle list
    /// </summary>
    /// <param name="vertices">A list of vertices of the chunk mesh</param>
    /// <param name="triangles">A list of triangle indices of the chunk mesh</param>
    /// <param name="pos">A 3D point in world space (bottom left corner of the quad)</param>
    /// <param name="widthDir">Direction of width for quad</param>
    /// <param name="lengthDir">Direction of length for quad</param>
    /// <param name="normal">Normal vector of quad face</param>
    public static void AddQuad(List<Vector3> vertices, List<int> triangles, Vector3 pos, Vector3 widthDir, Vector3 lengthDir, Vector3 normal)
    {
        //Calculate top and bottom left, right vertex positions based on given direction vectors
        Vector3 vBottomLeft = Vector3.zero, vBottomRight = Vector3.zero, vTopLeft = Vector3.zero, vTopRight = Vector3.zero;

        vBottomLeft = pos;
        vBottomRight = pos + widthDir;
        vTopLeft = pos + lengthDir;
        vTopRight = pos + widthDir + lengthDir;

        //If normal vector is left or forward flip the triangle to render on correct side (related to winding order)
        int vIndex = vertices.Count;
        if (normal == Vector3.left || normal == Vector3.forward || normal == Vector3.down)
        {
            //Add the triangle indices to the list in a counter-clockwise order
            triangles.Add(vIndex);
            triangles.Add(vIndex + 1);
            triangles.Add(vIndex + 2);

            triangles.Add(vIndex + 2);
            triangles.Add(vIndex + 1);
            triangles.Add(vIndex + 3);
        }
        else
        {
            //Add the triangle indices to the list in a clockwise order
            triangles.Add(vIndex);
            triangles.Add(vIndex + 2);
            triangles.Add(vIndex + 1);

            triangles.Add(vIndex + 2);
            triangles.Add(vIndex + 3);
            triangles.Add(vIndex + 1);
        }

        //Add the calculated vertices to the vertex list
        vertices.Add(vBottomLeft);
        vertices.Add(vBottomRight);
        vertices.Add(vTopLeft);
        vertices.Add(vTopRight);
    }
    /// <summary>
    /// Generate the byte value at the given world position with the current NoiseSettings
    /// </summary>
    /// <param name="pos">A 3D point in world space</param>
    /// <param name="settings">A NoiseSettings struct containing the settings for the octave noise</param>
    /// <returns>A byte value (0 for air, 1 for a solid block)</returns>
    public static byte GenerateBlock(Vector3 pos, NoiseSettings settings)
    {
        byte output = 0;
        //Get layered noise value at the world position with current settings, subtracting block pos.y
        //NOTE: subtracting pos.y is done to create a flat plane near y = 0
        float val = LayeredNoise(pos,settings) - pos.y;

        //If the value is greater than the air block cutoff it is a solid block (set the output to 1)
        if(val > settings.cutoff)
        {
            output = 1;
        }

        //Duplicate the noise settings (since structs are not references) and change the scale value used to the cave scale stored in settings
        NoiseSettings caveSettings = settings;
        caveSettings.scale = caveSettings.caveScale;

        //Get layered noise value at the world position with noise settings to generate caves
        //NOTE: Absolute value of noise is taken to achieve a cave like appearance
        //(large negative noise values will create solid blocks instead of air)
        val = Mathf.Abs(LayeredNoise(pos,caveSettings));

        //We want to layer the cave noise over the current terrain noise (so the caves can cut through the mountains/valleys that have already generated)
        //So if the output has been set to a solid block and the the generated cave noise value is less than a given caveCutoff replace that block with air
        //NOTE: Increasing the caveCutoff value will create wider cave tunnels, along with the valleys that are already generated by the first layered noise step, so keep this cave cutoff value low
        //(If output already = 0 then there is already an air block here and no point in writing a 0 byte again)
        if(output == 1 && val < caveSettings.caveCutoff)
        {
            output = 0;
        }
        return output;
    }

    /// <summary>
    /// Convert a given local position to a 1D index for the byte array
    /// </summary>
    /// <param name="pos">A 3D point in local space</param>
    /// <param name="chunkSize">The world scale of the chunk</param>
    /// <returns>A integer index for the byte array</returns>
    public static int GetByteArrayIndex(Vector3 pos, Vector3Int chunkSize)
    {
        //IMPORTANT: This index is based on the loops in the byte array generation (NOTE: If chunk width = height = depth order of multiplication doesnt matter)
        //If order of loops is changed when generating the byte array and chunk dimensions are not all equal this equation needs to be updated
        return (int)pos.x + chunkSize.x * ((int)pos.y + chunkSize.y * (int)pos.z);
    }
    /// <summary>
    /// Generate a 3D Perlin Noise value at the given world space position
    /// </summary>
    /// <param name="x">A world space x coordinate</param>
    /// <param name="y">A world space y coordinate</param>
    /// <param name="z">A world space z coordinate</param>
    /// <returns>A 3D Perlin Noise value betweeen 0.0 and 1.0</returns>
    public static float PerlinNoise3D(float x, float y, float z)
    {
        //This is quick hack to generate semi-accurate 3D noise using only 2D noise values
        //Fairly expensive as 6 calls to perlin noise need to be called
        //I wanted to keep the code fairly contained within the bounds of C#/Unity without importing extra packages/libraries
        //The code for this faux 3D noise can be found here: https://www.youtube.com/watch?v=Aga0TBJkchM
        float xy = Mathf.PerlinNoise(x, y);
        float xz = Mathf.PerlinNoise(x, z);
        float yz = Mathf.PerlinNoise(y, z);
        float yx = Mathf.PerlinNoise(y, x);
        float zx = Mathf.PerlinNoise(z, x);
        float zy = Mathf.PerlinNoise(z, y);

        return (xy + xz + yz + yx + zx + zy) / 6;
    }
    /// <summary>
    /// Generate a 3D layered noise value at the given world space position with the given settings.
    /// NOTE: Upper bound on noise value returned varies with noise settings.
    /// </summary>
    /// <param name="pos">A 3D position in world space</param>
    /// <param name="settings">A NoiseSettings struct containing the settings for the layered noise</param>
    /// <returns>A 3D layered noise value >= 0.0</returns>
    public static float LayeredNoise(Vector3 pos, NoiseSettings settings)
    {
        /*
         * Code obtained from: https://www.youtube.com/watch?v=MRNFcywkUSA
         */
        float noiseValue = 0;
        float frequency = settings.baseRoughness;
        float amplitude = 1;
        for(int i = 0; i < settings.layers; i++)
        {
            Vector3 samplePos = new Vector3(pos.x/settings.scale * frequency + settings.seed, pos.y/settings.scale * frequency + settings.seed, pos.z/settings.scale * frequency + settings.seed);
            float v = PerlinNoise3D(samplePos.x, samplePos.y, samplePos.z);
            noiseValue += v * amplitude;
            frequency *= settings.roughness;
            amplitude *= settings.persistence;
        }

        noiseValue = Mathf.Max(0, noiseValue - settings.recede);
        return noiseValue * settings.strength;
    }
    //Container class for storing the chunk information (used to pass data between threads)
    //(Mesh vertices, triangles, arrays, world position, noise settings, saved data)
    class ChunkData
    {
        //List of vertex positions for the chunk mesh
        public List<Vector3> vertices = new List<Vector3>();
        //List of triangle indices for the chunk mesh
        public List<int> triangles = new List<int>();

        //Array representations of vertex/triangle lists
        public Vector3[] vertexArray;
        public int[] triangleArray;

        //3D chunk position in world space
        public Vector3 chunkWorldPos;
        //Chunk world scale
        public Vector3Int chunkSize;
        //Current noise settings
        public NoiseSettings settings;
        //A 1D array containing the byte values for each block in the chunk
        public byte[] byteArr;

        //Saved chunk data object (contains dictionary of destroyed/placed blocks)
        public SavedChunkData savedData;
        public ChunkData(Vector3 chunkWorldPos,  Vector3Int chunkSize, NoiseSettings noiseSettings, int byteArraySize, SavedChunkData savedData)
        {
            this.chunkWorldPos = chunkWorldPos;
            this.chunkSize = chunkSize;
            this.settings = noiseSettings;
            this.byteArr = new byte[byteArraySize];
            this.savedData = savedData;
        }
        /// <summary>
        /// Get the byte value at the given byte array index
        /// </summary>
        /// <param name="i">An integer index for the byte array</param>
        /// <returns>The byte value at index in the byte array</returns>
        public byte GetByteValue(int i)
        {
            return byteArr[i];
        }
        /// <summary>
        /// Set the byte value at the given byte array index to the given byte.
        /// </summary>
        /// <param name="i">An integer index for the byte array</param>
        /// <param name="b">The byte value to store in the array at the given index</param>
        public void SetByteValue(int i, byte b)
        {
            this.byteArr[i] = b;
        }
        /// <summary>
        /// Clears the vertex and triangle list
        /// </summary>
        public void Clear()
        {
            vertices.Clear();
            triangles.Clear();
        }
    }
    //Container class for storing saved chunk information (blocks placed/destroyed)
    public class SavedChunkData
    {
        //Dictionary of saved bytes, where key = their 
        Dictionary<Vector3Int, byte> storedBytes = new Dictionary<Vector3Int, byte>();
        //3D chunk x,y,z index
        Vector3Int chunkPos;
        public SavedChunkData(Vector3Int chunkPos)
        {
            this.chunkPos = chunkPos;
        }
        /// <summary>
        /// Add a byte to the stored bytes dictionary at a given local position.
        /// </summary>
        /// <param name="pos">A 3D position in local space</param>
        /// <param name="b">The byte value to add</param>
        public void AddByte(Vector3Int pos, byte b)
        {
            //If the dictionary already has a block saved here update the value, else add the byte as a new value
            if (storedBytes.ContainsKey(pos))
            {
                storedBytes[pos] = b;
            }
            else
            {
                storedBytes.Add(pos, b);
            }
        }
        /// <summary>
        /// Get the byte at a given local position.
        /// </summary>
        /// <param name="pos">A 3D position in local space</param>
        /// <returns>The byte value at the given position (if position does not exist, return 0)</returns>
        public byte GetByte(Vector3Int pos)
        {
            //Verify the dictionary has a byte at this position, if it does return the value, else return 0
            if (HasByte(pos))
                return storedBytes[pos];
            return 0;
        }
        /// <summary>
        /// Determine if dictionary contains byte at given local position.
        /// </summary>
        /// <param name="pos">A 3D position in local space</param>
        /// <returns>true if byte value exists given local position, false otherwise</returns>
        public bool HasByte(Vector3Int pos)
        {
            return storedBytes.ContainsKey(pos);
        }
        /// <summary>
        /// Get the 3D chunk x,y,z index
        /// </summary>
        /// <returns>A 3D chunk index vector</returns>
        public Vector3Int GetChunkPos()
        {
            return this.chunkPos;
        }
    }
}