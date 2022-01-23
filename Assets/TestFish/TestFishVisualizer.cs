using UnityEngine;

namespace MarchingCubes
{

    public sealed class TestFishVisualizer : MonoBehaviour
    {
        #region Editable attributes

        [SerializeField] Vector3Int _dimensions = new Vector3Int(64, 32, 64);
        [SerializeField] float _gridScale = 4.0f / 64;
        [SerializeField] int _triangleBudget = 65536;
        [SerializeField] float _targetValue = 0;

        #endregion

        #region Project asset references

        [SerializeField] ComputeShader _volumeCompute = null;
        [SerializeField] ComputeShader _builderCompute = null;
        public Vector3[] RetrievedVertices;
        public int[] RetrievedIndices;
        #endregion

        #region Private members

        int VoxelCount => _dimensions.x * _dimensions.y * _dimensions.z;

        ComputeBuffer _voxelBuffer;
        MeshBuilder _builder;

        #endregion

        #region MonoBehaviour implementation

        void Start()
        {
            RetrievedIndices = null;
            RetrievedVertices = null;
            _voxelBuffer = new ComputeBuffer(VoxelCount, sizeof(float));
            _builder = new MeshBuilder(_dimensions, _triangleBudget, _builderCompute);

        }

        void OnDestroy()
        {
            _voxelBuffer.Dispose();
            _builder.Dispose();
        }

        void Update()
        {
            // Noise field update
            _volumeCompute.SetInts("Dims", _dimensions);
            _volumeCompute.SetFloat("Scale", _gridScale);
            _volumeCompute.SetFloat("Time", Time.time);
            _volumeCompute.SetBuffer(0, "Voxels", _voxelBuffer);
            _volumeCompute.DispatchThreads(0, _dimensions);

            // Isosurface reconstruction
            _builder.BuildIsosurface(_voxelBuffer, _targetValue, _gridScale);
            if (RetrievedIndices == null)
            {
                RetrievedVertices = new Vector3[_builder._vertexBuffer.count];
                RetrievedIndices = new int[_builder._indexBuffer.count];
            }
            _builder._indexBuffer.GetData(RetrievedIndices);
            _builder._vertexBuffer.GetData(RetrievedVertices);
            GetComponent<MeshFilter>().sharedMesh = _builder.Mesh;
        }

        #endregion
    }

} // namespace MarchingCubes
