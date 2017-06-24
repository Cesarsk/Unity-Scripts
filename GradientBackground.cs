using UnityEngine;

//this is a enhanced and updated version (works with Unity 5.6.1f1) of a script which allows users 
//to set a gradient background in their camera component

//this version of the script can change color dynamically (runtime) 
//and loads a shader called Vertex Color Only.shader placed in assets/materials/

public class GradientBackground : MonoBehaviour
{
    public Color topColor = Color.blue;
    public Color bottomColor = Color.white;
    public int gradientLayer = 7;
    public static bool updateColorRuntime = false;
    Mesh mesh;

    private void Update()
    {
        if (!updateColorRuntime)
        {
            mesh.colors = new Color[4] { topColor, topColor, bottomColor, bottomColor };
            //if you need to update just once, you can disable the variable after used it
            updateColorRuntime = false;
        }
    }

    void Awake()
    {
        gradientLayer = Mathf.Clamp(gradientLayer, 0, 31);
        if (!GetComponent<Camera>())
        {
            Debug.LogError("Must attach GradientBackground script to the camera");
            return;
        }

        GetComponent<Camera>().clearFlags = CameraClearFlags.Depth;
        GetComponent<Camera>().cullingMask = GetComponent<Camera>().cullingMask & ~(1 << gradientLayer);
        Camera gradientCam = new GameObject("Gradient Cam", typeof(Camera)).GetComponent<Camera>();
        gradientCam.depth = GetComponent<Camera>().depth - 1;
        gradientCam.cullingMask = 1 << gradientLayer;

        mesh = new Mesh();
        mesh.vertices = new Vector3[4]
                        {new Vector3(-100f, .577f, 1f), new Vector3(100f, .577f, 1f), new Vector3(-100f, -.577f, 1f), new Vector3(100f, -.577f, 1f)};

        mesh.colors = new Color[4] { topColor, topColor, bottomColor, bottomColor };

        mesh.triangles = new int[6] { 0, 1, 2, 1, 3, 2 };
        Shader shader = Shader.Find("Vertex Color Only");
        Material mat = new Material(shader);
        GameObject gradientPlane = new GameObject("Gradient Plane", typeof(MeshFilter), typeof(MeshRenderer));

        ((MeshFilter)gradientPlane.GetComponent(typeof(MeshFilter))).mesh = mesh;
        gradientPlane.GetComponent<Renderer>().material = mat;
        gradientPlane.layer = gradientLayer;
    }
}
