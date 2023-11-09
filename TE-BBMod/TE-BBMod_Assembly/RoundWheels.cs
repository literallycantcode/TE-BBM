using System;
using System.Linq;
using Modding;
using Modding.Blocks;
using static Modding.ModKeys;
using System.Collections;
using Console = Modding.ModConsole;
using UnityEngine;

namespace MysticFix
{

    public class RoundWheels : MonoBehaviour
    {
        public BlockBehaviour BB;
        private MeshCollider mCollider;
        public GameObject WheelCollider;
        public CogMotorControllerHinge CCH;
        private Rigidbody rigg;
        static GameObject WheelColliderOrgin;
        private Collider[] Colliders;
        private MeshFilter mFilter;
        public bool MakeRound = false; 
        public MToggle Roundwheelz;
        private bool isFirstFrame = true;

        public void Awake()
        {
            Console.Log("Round Wheels Added to "+gameObject.name);
            BB = GetComponent<BlockBehaviour>();

            //1.2.5 update compat
            if (WheelColliderOrgin == null)
            {
                StartCoroutine(ReadWheelMesh());
            }

            //Mapper definition
            Roundwheelz = BB.AddToggle("ROUNDWHEELZ!", "ROUNDWHEELZ!", MakeRound);
            Roundwheelz.Toggled += (bool value) => { MakeRound = value; };

            //DisplayInMapper config
            Roundwheelz.DisplayInMapper = true;

            //Physics stuff
            if (!StatMaster.isClient || StatMaster.isLocalSim)
            {
                rigg = GetComponent<Rigidbody>();
            }
        }

        private struct PaS
        {
            public Vector3 Position;
            public Vector3 Scale;
            public static PaS one = new PaS { Position = Vector3.one, Scale = Vector3.one};

            public static PaS GetPositionScaleAndFriction(string name)
            {
                PaS psaf = new PaS();

                if (name == "Wheel")
                {
                    psaf.Position = new Vector3(0, 0, 0.19f);
                    psaf.Scale = new Vector3(0.99f, 0.99f, 0.17f);
                    return psaf;
                }
                if (name == "LargeWheel")
                {
                    psaf.Position = new Vector3(0, 0, 0.46f);
                    psaf.Scale = new Vector3(1.41f, 1.41f, 0.33f); //correct is 1.46, only not change because of SHWS
                    return psaf;
                }
                if (name == "WheelUnpowered")
                {
                    psaf.Position = new Vector3(0, 0, 0.185f);
                    psaf.Scale = new Vector3(0.99f, 0.99f, 0.18f); 
                    return psaf;
                }
                if (name == "LargeWheelUnpowered")
                {
                    psaf.Position = new Vector3(0, 0, 0.46f);
                    psaf.Scale = new Vector3(1.41f, 1.41f, 0.26f);
                    return psaf;
                }
                return PaS.one;
            }
        }

        void Update()
        {
            if (!StatMaster.isClient || StatMaster.isLocalSim)
            {
                if (BB.isSimulating)
                {
                    if (isFirstFrame)
                    {
                        isFirstFrame = false;
                        if (!rigg)
                            return;

                        Colliders = GetComponentsInChildren<Collider>();
                        if (MakeRound)
                        {
                            if (WheelCollider != null) return;

                            foreach (Collider c in Colliders) { if (c.name == "CubeColliders") c.isTrigger = true; }

                            WheelCollider = (GameObject)Instantiate(WheelColliderOrgin, transform.position, transform.rotation, transform);
                            WheelCollider.SetActive(true);
                            WheelCollider.name = "Wheel Collider";
                            WheelCollider.transform.SetParent(transform);

                            mFilter = WheelCollider.AddComponent<MeshFilter>();
                            mFilter.sharedMesh = WheelCollider.GetComponent<MeshCollider>().sharedMesh;

                            mCollider = WheelCollider.GetComponent<MeshCollider>();
                            mCollider.convex = true;

                            PaS pas = PaS.GetPositionScaleAndFriction(gameObject.name);

                            WheelCollider.transform.parent = transform;
                            WheelCollider.transform.rotation = transform.rotation;
                            WheelCollider.transform.position = transform.TransformPoint(transform.InverseTransformPoint(transform.position) + pas.Position);
                            WheelCollider.transform.localScale = pas.Scale;
                        }
                        else
                        {
                            Destroy(WheelCollider);
                        }
                    }
                }
            }
        }

        static IEnumerator ReadWheelMesh()
        {
            //Debug.Log("Readmesh!");
            WheelColliderOrgin = new GameObject("Wheel Collider Orgin");
            DontDestroyOnLoad(WheelColliderOrgin);
            //WheelColliderOrgin.transform.SetParent(Mod.gameObject.transform);
            
            if (!WheelColliderOrgin.GetComponent<MeshCollider>())
            {
                Besiege.AssetImporter.readableMeshes = true;
                ModMesh modMesh = ModResource.CreateMeshResource("WheelMesh", "Resources/Wheel.obj");
                MeshCollider meshCollider = WheelColliderOrgin.AddComponent<MeshCollider>();

                yield return new WaitUntil(() => modMesh.Available);

                meshCollider.sharedMesh = ModResource.GetMesh("WheelMesh");
                meshCollider.convex = true;
                WheelColliderOrgin.SetActive(false);

                //1.25 update compat
                Besiege.AssetImporter.readableMeshes = false;
            }
        }
    }
}