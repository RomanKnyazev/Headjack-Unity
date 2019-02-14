#if HEADJACK_GOT_PLUGINS
using UnityEngine;
namespace Headjack
{
    public class ViveControllerManager : MonoBehaviour 
    {
        public GameObject[] Laser;
        public Shader controllerShader, LaserShader;
        private SteamVR_TrackedController[] controller;
    	void Start ()
        {
            controllerShader = (Shader)Resources.Load<Shader>("Shaders/Remote");
            LaserShader = (Shader)Resources.Load<Shader>("Shaders/Laser");


            gameObject.AddComponent<SteamVR_PlayArea>();
            gameObject.AddComponent<MeshFilter>();
            gameObject.AddComponent<MeshRenderer>();
            SteamVR_ControllerManager cm = gameObject.AddComponent<SteamVR_ControllerManager>();

            Laser = new GameObject[2];
            controller = new SteamVR_TrackedController[2];

            GameObject left = new GameObject();
            left.name = "Controller Left";
            left.transform.parent = transform;
            left.AddComponent<SteamVR_TrackedObject>();
            controller[0] = left.AddComponent<SteamVR_TrackedController>();
            GameObject leftModel = new GameObject();
            leftModel.name = "Model";
            leftModel.transform.parent = left.transform;
            SteamVR_RenderModel lr = leftModel.AddComponent<SteamVR_RenderModel>();
            lr.shader = controllerShader;
            GameObject leftLaser = new GameObject();
            leftLaser.name = "Laser";
            leftLaser.transform.parent = left.transform;
            GameObject leftLaserMesh = GameObject.CreatePrimitive(PrimitiveType.Cube);
            Destroy(leftLaserMesh.GetComponent<BoxCollider>());
            leftLaserMesh.transform.parent = leftLaser.transform;
            leftLaserMesh.name = "LaserMesh";
            leftLaserMesh.GetComponent<MeshRenderer>().material = new Material(LaserShader);
            leftLaserMesh.transform.localPosition = new Vector3(0, 0, 0.5f);
            leftLaserMesh.transform.localScale = new Vector3(0.005f, 0.005f, 1f);
            Laser[0] = leftLaser;

            GameObject right = new GameObject();
            right.name = "Controller Right";
            right.transform.parent = transform;
            right.AddComponent<SteamVR_TrackedObject>();
            controller[1] = right.AddComponent<SteamVR_TrackedController>();
            GameObject rightModel = new GameObject();
            rightModel.name = "Model";
            rightModel.transform.parent = right.transform;
            SteamVR_RenderModel rr = rightModel.AddComponent<SteamVR_RenderModel>();
            rr.shader = controllerShader;
            GameObject rightLaser = new GameObject();
            rightLaser.name = "Laser";
            rightLaser.transform.parent = right.transform;
            GameObject rightLaserMesh = GameObject.CreatePrimitive(PrimitiveType.Cube);
            Destroy(rightLaserMesh.GetComponent<BoxCollider>());
            rightLaserMesh.transform.parent = rightLaser.transform;
            rightLaserMesh.name = "LaserMesh";
            rightLaserMesh.GetComponent<MeshRenderer>().material = new Material(LaserShader);
            rightLaserMesh.transform.localPosition = new Vector3(0, 0, 0.5f);
            rightLaserMesh.transform.localScale = new Vector3(0.005f, 0.005f, 1f);
            Laser[1] = rightLaser;

            transform.parent.GetComponent<VRInput>().Laser = Laser;

            cm.left = left;
            cm.right = right;
            cm.objects = new GameObject[2];
            cm.objects[0] = left;
            cm.objects[1] = right;

            for (int i = 0; i < 2; ++i)
            {
                int p = i;
                controller[p].TriggerClicked += new ClickedEventHandler(delegate(object sender, ClickedEventArgs e)
                    {
                        VRInput.Button(p, VRInput.ButtonID.Trigger, true);
                    });
                controller[p].TriggerUnclicked += new ClickedEventHandler(delegate(object sender, ClickedEventArgs e)
                    {
                        VRInput.Button(p, VRInput.ButtonID.Trigger, false);
                    });

                controller[p].MenuButtonClicked += new ClickedEventHandler(delegate(object sender, ClickedEventArgs e)
                    {
                        VRInput.Button(p, VRInput.ButtonID.Back, true);
                    });
                controller[p].MenuButtonUnclicked += new ClickedEventHandler(delegate(object sender, ClickedEventArgs e)
                    {
                        VRInput.Button(p, VRInput.ButtonID.Back, false);
                    });
            }
        }

    }
}
#endif