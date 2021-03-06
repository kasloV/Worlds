﻿using System.Collections.Generic;
using UnityEngine;
using Worlds.ProceduralTerrain.Generator.Engine;
using Worlds.ProceduralTerrain;
using UnityEditor.VersionControl;

public class Surface : MonoBehaviour
{
    public int m_num_of_chunks = 1;
    public int m_chunk_res = 16;
    public float m_radius;
    [Range(0.1f, 3f)] public float m_sigma = 0.5f;
    public int m_kernelSize = 3;

    public int m_mesh_res { get; private set; }
    public int m_surface_res { get; private set; }
    public SurfaceLayer m_surface { get; private set; }

    private List<GameObject> m_chunks;

    public GameObject surfaceChunkPrefab;
    public GameObject surfaceMapTexturePrefab;
    void Start()
    {
        m_mesh_res = m_num_of_chunks * m_chunk_res;
        m_surface_res = m_mesh_res + 1;
        m_surface = new SurfaceLayer(m_surface_res);
        SurfaceLayer sphere = SurfaceBrush.Sphere(new Vector3Int(5, 5, 5), m_radius);
        SurfaceLayer innersphere = SurfaceBrush.Sphere(new Vector3Int(8, 8, 8), m_radius / 3f);
        SurfaceLayer outersphere = SurfaceBrush.Sphere(new Vector3Int(10, 20, 8), m_radius / 3f);
        m_surface.Merge(sphere, 2f, SurfaceLayer.MergeMethod.Overlay, SurfaceLayer.MergeSize.Cut);
        m_surface.Merge(innersphere, 2f, SurfaceLayer.MergeMethod.Subtract, SurfaceLayer.MergeSize.Cut);
        m_surface.Merge(outersphere, 2f, SurfaceLayer.MergeMethod.Add, SurfaceLayer.MergeSize.Cut);
        m_surface.Filter(FilterKernel3D.Gaussian(m_kernelSize, m_sigma));
        //m_surface.Filter(FilterKernel3D.Mean(m_kernelSize));
        //SurfaceLayer tetrahedron = SurfaceBrush.Tetrahedron(Vector3Int.zero, new Vector3Int(5, 0, 0)*3, new Vector3Int(0, 0, 5)*3, new Vector3Int(2, 5, 2)*3);
        //m_surface.Merge(tetrahedron, 2f, SurfaceLayer.MergeMethod.Add, SurfaceLayer.MergeSize.Cut);
        //SurfaceLayer tetra = SurfaceBrush.Tetrahedron(new Vector3Int(0, 0, 0), new Vector3Int(5, 0, 0), new Vector3Int(0, 0, 5), new Vector3Int(2, 5, 2), m_fill);
        //m_surface = SurfaceLayer.Merge(m_surface, tetra, 2f, SurfaceLayer.MergeMethod.Overlay, SurfaceLayer.MergeSize.Cut);

        m_chunks = new List<GameObject>();
        for(int z = 0; z < m_num_of_chunks; z++)
        {
            for(int y = 0; y < m_num_of_chunks; y++)
            {
                for(int x = 0; x < m_num_of_chunks; x++)
                {
                    int index = x + y * m_num_of_chunks + z * m_num_of_chunks * m_num_of_chunks;
                    GameObject chunk = Instantiate(surfaceChunkPrefab, transform.position + new Vector3(x * m_chunk_res, y * m_chunk_res, z * m_chunk_res), Quaternion.identity, transform);
                    chunk.name = name + "_" + index.ToString();
                    m_chunks.Add(chunk);
                    chunk.GetComponent<SurfaceChunk>().Initalize(index);
                    chunk.GetComponent<SurfaceChunk>().Refresh();
                }
            }
        }
        Instantiate(surfaceMapTexturePrefab, transform.position + new Vector3(-20.3f, 1.12f, 0f), Quaternion.Euler(90f, -180f, 0), transform);
    }

    private void Update()
    {
        //SurfaceLayer sphere = SurfaceBrush.Sphere(Vector3Int.zero, m_radius, 1f);
        //m_surface = SurfaceLayer.Merge(m_surface, sphere, 2f, SurfaceLayer.MergeMethod.Overlay, SurfaceLayer.MergeSize.Cut);
        //m_surface.Filter(FilterKernel3D.Gaussian(m_kernelSize, m_sigma));
        //SurfaceLayer tetrahedron = SurfaceBrush.Tetrahedron(Vector3Int.zero, new Vector3Int(20, 0, 0), new Vector3Int(0, 0, 20), new Vector3Int(8, 20, 8));
        //m_surface.Merge(tetrahedron, 2, SurfaceLayer.MergeMethod.Overlay, SurfaceLayer.MergeSize.Cut);
        //m_surface.Filter(FilterKernel3D.Gaussian(m_kernelSize, m_sigma));
    }
}
