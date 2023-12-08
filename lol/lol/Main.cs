namespace lol
{
    using JustPlay.Equipment;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Threading.Tasks;
    using UnityEngine;

    public class Main : MonoBehaviour
    {
        public Settings cSettings = new Settings();
        private int screenWidth = Screen.width;
        private int screenHeight = Screen.height;
        private Main thisObject;
        private Quaternion currentRotation;
        private bool gotFirstRotation;
        private Vector3 weaponOriginPosition = Vector3.zero;
        private KDNONBOEMLD currentWeapon;
        private Vector3 closestPlayer2 = Vector3.zero;
        private Vector3 closestPlayer3 = Vector3.zero;
        private Vector3 closesntHead3 = Vector3.zero;
        private bool closestPlayerVisible;
        public static Dictionary<int, PlayerController> playerControllers = new Dictionary<int, PlayerController>();
        private SkinnedMeshRenderer[] skinnedMeshRenderers;
        private SkinnedMeshRenderer mySkinnedMeshRenderer;
        private Texture2D logo;
        private float _currCollider;
        private float _capsuleCollider;
        private float _crawlCollider;
        private float _legsCollider;
        private float PlayerMainCollider;
        private Transform closestChest;
        public static Dictionary<string, Font> Fonts = new Dictionary<string, Font>();
        public static Dictionary<string, UnityEngine.Shader> Shader = new Dictionary<string, UnityEngine.Shader>();
        public static Dictionary<string, UnityEngine.Shader> oldShaders = new Dictionary<string, UnityEngine.Shader>();
        public static Font _textFont;
        public static UnityEngine.Shader _Chams;
        public static UnityEngine.Shader _ColorChams;
        public static UnityEngine.Shader _Pulsing;
        public static UnityEngine.Shader _Rainbow;
        public static UnityEngine.Shader _Wireframe;
        public string assetName;
        private string tempFilePath;
        private bool setup;
        private bool needToKIll;
        private bool inFreeCamMode;
        private Dictionary<int, string> idToNameMapping = new Dictionary<int, string>();
        private bool changeName;
        private Vector3 debug = Vector3.zero;
        private static string debugString = "";
        private static List<string> debugStrings = new List<string>();
        private PlayerController closestController;
        private System.Random rand = new System.Random();
        private int debugN;
        private int tempN;
        private bool resetSpeed;
        private float defaultSpeed = float.NegativeInfinity;
        private Vector3 myFreeCamPos = Vector3.zero;
        private Quaternion myFreeCamRot;
        public const float PI = 3.141593f;
        private int tabb = 2;
        private GUIContent navBackButtonTexture;

        public static Texture2D Base64ToTexture(string encodedData)
        {
            byte[] data = Convert.FromBase64String(encodedData);
            Texture2D tex = new Texture2D(1, 1, TextureFormat.ARGB32, true, true);
            tex.hideFlags = HideFlags.HideAndDontSave;
            tex.filterMode = FilterMode.Bilinear;
            tex.LoadImage(data, true);
            return tex;
        }

        public void drawBox(Vector2 p1, Vector2 p2)
        {
            this.drawBox(p1, p2, Color.magenta);
        }

        public void drawBox(Vector2 p1, Vector2 p2, Color color)
        {
            Drawing.DrawLine(p1, new Vector2(p2.x, p1.y), color, 1f, true);
            Drawing.DrawLine(p1, new Vector2(p1.x, p2.y), color, 1f, true);
            Drawing.DrawLine(p2, new Vector2(p2.x, p1.y), color, 1f, true);
            Drawing.DrawLine(p2, new Vector2(p1.x, p2.y), color, 1f, true);
        }

        private void drawLineFromTo(Vector3 pos, Vector3 pos2)
        {
            this.drawLineFromTo(pos, pos2, Color.red);
        }

        private void drawLineFromTo(Vector3 pos1, Vector3 pos2, Color color)
        {
            Vector3 pointA = Camera.main.WorldToScreenPoint(pos1);
            Vector3 pointB = Camera.main.WorldToScreenPoint(pos2);
            if ((pointA.z > 0f) && (pointB.z > 0f))
            {
                pointA.y = this.screenHeight - pointA.y;
                pointB.y = this.screenHeight - pointB.y;
                Drawing.DrawLine(pointA, pointB, color, 2f, true);
            }
        }

        private void drawLineToPos(Vector3 pos)
        {
            this.drawLineToPos(pos, Color.red);
        }

        private void drawLineToPos(Vector3 pos, Color color)
        {
            Vector3 pointB = Camera.main.WorldToScreenPoint(pos);
            if (pointB.z > 0f)
            {
                pointB.y = this.screenHeight - pointB.y;
                Drawing.DrawLine(new Vector2((float) (this.screenWidth / 2), (float) this.screenHeight), pointB, color, 2f, true);
            }
        }

        private void drawText(Vector3 pos, string text)
        {
            lol.Renderer.DrawString(new Vector2(pos.x, pos.y), text, Color.white, true);
        }

        private void drawText(Vector3 pos, string text, Color color)
        {
            lol.Renderer.DrawString(new Vector2(pos.x, pos.y), text, color, true);
        }

        protected void FixedUpdate()
        {
            if (this.cSettings.skinChanger && (this.nameFromId(this.cSettings.skinId) != "Invalid"))
            {
                FirebaseManager.KODEGOFIJIC.BPBOJJGBICI.Skins.EquippedCharacterSkin = "lol.1v1.playerskins.pack." + this.cSettings.skinId.ToString();
            }
            if (this.cSettings.MemoryAim)
            {
                CameraManager kODEGOFIJIC = CameraManager.KODEGOFIJIC;
                if (this.IsInScreen(this.closestPlayer2, 0) && ((this.closestPlayer2 != Vector3.zero) && Input.GetMouseButton(1)))
                {
                    Quaternion b = Quaternion.LookRotation((this.closestPlayer3 - Camera.main.transform.position).normalized);
                    Vector3 eulerAngles = Quaternion.Slerp(Quaternion.Euler((Vector3) kODEGOFIJIC.TPCamera.GetRotation()), b, this.cSettings.smooth).eulerAngles;
                    eulerAngles.x = this.fixValue(eulerAngles.x);
                    kODEGOFIJIC.TPCamera.SetRotation(eulerAngles);
                }
            }
        }

        private float fixValue(float value)
        {
            float num = value;
            if ((value >= 0f) && (value < 85f))
            {
                return ((value <= 80f) ? num : 80f);
            }
            if (value > 85f)
            {
                num = value - 360f;
                if (value < -89f)
                {
                    num = -89f;
                }
            }
            return num;
        }

        private Transform GetBone(Transform[] bones, string name)
        {
            foreach (Transform transform in bones)
            {
                if (transform.name == name)
                {
                    return transform;
                }
            }
            return null;
        }

        private bool IsInScreen(Vector3 pos, int over = 30) => 
            (pos.x > -over) && ((pos.x < (this.screenWidth + over)) && ((pos.y > -over) && (pos.y < (this.screenHeight + over))));

        public static bool IsVisible(Vector3 playerPos)
        {
            RaycastHit hit;
            Vector3 vector = playerPos - Camera.main.transform.position;
            Vector3 normalized = vector.normalized;
            if (!Physics.Raycast(Camera.main.transform.position, normalized, out hit, vector.magnitude, (~(1 << (LayerMask.NameToLayer("Zone") & 0x1f)) | ~(1 << (LayerMask.NameToLayer("Water") & 0x1f))) | ~(1 << (LayerMask.NameToLayer("Building Edit") & 0x1f))))
            {
                return false;
            }
            string str = LayerMask.LayerToName(hit.collider.gameObject.layer);
            return ((str == "OurPlayer") || ((str == "AutoAim") || (str == "PlayerDetection")));
        }

        protected void LateUpdate()
        {
            CameraManager kODEGOFIJIC = CameraManager.KODEGOFIJIC;
            if (!this.inFreeCamMode)
            {
                kODEGOFIJIC.TPCamera.shouldMoveCamera = true;
                this.myFreeCamPos = Vector3.zero;
                this.myFreeCamRot = new Quaternion();
            }
            else
            {
                kODEGOFIJIC.TPCamera.shouldMoveCamera = false;
                vThirdPersonCamera tPCamera = kODEGOFIJIC.TPCamera;
                if (this.myFreeCamPos == Vector3.zero)
                {
                    this.myFreeCamPos = tPCamera.currentTarget.position;
                    this.myFreeCamRot = tPCamera.currentTarget.rotation;
                }
                bool flag = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
                float num = flag ? 100f : 10f;
                if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
                {
                    tPCamera.transform.position += (-tPCamera.transform.right * num) * Time.deltaTime;
                }
                if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
                {
                    tPCamera.transform.position += (tPCamera.transform.right * num) * Time.deltaTime;
                }
                if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
                {
                    tPCamera.transform.position += (tPCamera.transform.forward * num) * Time.deltaTime;
                }
                if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
                {
                    tPCamera.transform.position += (-tPCamera.transform.forward * num) * Time.deltaTime;
                }
                if (Input.GetKey(KeyCode.Space))
                {
                    tPCamera.transform.position += (tPCamera.transform.up * num) * Time.deltaTime;
                }
                if (Input.GetKey(KeyCode.LeftControl))
                {
                    tPCamera.transform.position += (-tPCamera.transform.up * num) * Time.deltaTime;
                }
                if (!Input.GetKey(KeyCode.LeftAlt))
                {
                    float x = tPCamera.transform.localEulerAngles.x - (Input.GetAxis("Mouse Y") * 3f);
                    tPCamera.transform.localEulerAngles = new Vector3(x, tPCamera.transform.localEulerAngles.y + (Input.GetAxis("Mouse X") * 3f), 0f);
                }
                float axis = Input.GetAxis("Mouse ScrollWheel");
                if (axis != 0f)
                {
                    float num5 = flag ? 50f : 10f;
                    tPCamera.transform.position += (tPCamera.transform.forward * axis) * num5;
                }
                kODEGOFIJIC.TPCamera.currentTarget.position = this.myFreeCamPos;
                kODEGOFIJIC.TPCamera.currentTarget.rotation = this.myFreeCamRot;
            }
        }

        private Task LoadAssetBundleAsync2()
        {

            return null;
        }

        private void LoadSettings()
        {
            string path = Path.Combine(Application.persistentDataPath, "Settings.json");
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                this.cSettings = JsonUtility.FromJson<Settings>(json);
            }
        }

        public void mainWindow(int windowId)
        {
            GUIContent content;
            int num = 50;
            GUIStyle style = new GUIStyle(GUI.skin.toggle);
            GUIStyle style1 = new GUIStyle(GUI.skin.toggle);
            style1.fontSize = 15;
            style1.font = _textFont;
            GUIStyle style2 = style1;
            GUI.skin.label.font = _textFont;
            GUI.contentColor = Color.white;
            Rect position = new Rect(175f, 30f, 150f, 30f);
            Rect rect2 = new Rect(330f, 30f, 150f, 30f);
            if (GUI.Button(new Rect(20f, 30f, 150f, 30f), this.navBackButtonTexture))
            {
                this.tabb = 0;
            }
            if (GUI.Button(position, this.navBackButtonTexture))
            {
                this.tabb = 1;
            }
            if (GUI.Button(rect2, this.navBackButtonTexture))
            {
                this.tabb = 2;
            }
            GUI.contentColor = Color.red;
            GUI.skin.label.font = _textFont;
            GUI.skin.label.fontSize = 15;
            GUI.Label(new Rect(70f, 35f, 150f, 30f), "AIMBOT");
            GUI.Label(new Rect(240f, 35f, 150f, 30f), "ESP");
            GUI.Label(new Rect(375f, 35f, 150f, 30f), "EXPLOITS");
            GUI.contentColor = Color.white;
            if (this.tabb == 0)
            {
                content = new GUIContent("use fov");
                int num1 = num + 20;
                this.cSettings.useFov = GUI.Toggle(new Rect(10f, (float) (num = num1), 100f, 20f), this.cSettings.useFov, content, style);
                if (this.cSettings.useFov)
                {
                    content = new GUIContent("show fov");
                    int num3 = num + 20;
                    this.cSettings.showFov = GUI.Toggle(new Rect(10f, (float) (num = num3), 100f, 20f), this.cSettings.showFov, content, style);
                    int num4 = num + 20;
                    this.cSettings.fovSize = GUI.Slider(new Rect(10f, (float) (num = num4), 100f, 20f), this.cSettings.fovSize, 0.01f, 0.01f, 0.51f, GUI.skin.horizontalSlider, GUI.skin.horizontalSliderThumb, true, 3, null);
                }
                content = new GUIContent("memory aim");
                bool memoryAim = this.cSettings.MemoryAim;
                int num5 = num + 20;
                this.cSettings.MemoryAim = GUI.Toggle(new Rect(10f, (float) (num = num5), 100f, 20f), this.cSettings.MemoryAim, content, style);
                int num6 = num + 20;
                this.cSettings.smooth = GUI.Slider(new Rect(10f, (float) (num = num6), 100f, 20f), this.cSettings.smooth, 0.01f, 0f, 1.01f, GUI.skin.horizontalSlider, GUI.skin.horizontalSliderThumb, true, 4, null);
                GUI.Label(new Rect(110f, (float) (num - 2), 150f, 30f), "smooth: " + $"{this.cSettings.smooth:N2}");
                content = new GUIContent("silent aim");
                bool silentAim = this.cSettings.SilentAim;
                int num7 = num + 20;
                this.cSettings.SilentAim = GUI.Toggle(new Rect(10f, (float) (num = num7), 100f, 20f), this.cSettings.SilentAim, content, style);
                int num8 = num + 20;
                this.cSettings.hitChance = GUI.Slider(new Rect(10f, (float) (num = num8), 100f, 20f), this.cSettings.hitChance, 0.1f, 0f, 100.1f, GUI.skin.horizontalSlider, GUI.skin.horizontalSliderThumb, true, 0x275a4bd, null);
                GUI.Label(new Rect(110f, (float) (num - 2), 150f, 30f), "hitChance: " + $"{this.cSettings.hitChance:N1}" + "%");
                if (memoryAim && (!silentAim && this.cSettings.SilentAim))
                {
                    this.cSettings.MemoryAim = false;
                }
                else if (silentAim && (!memoryAim && this.cSettings.MemoryAim))
                {
                    this.cSettings.SilentAim = false;
                }
                content = new GUIContent("flick (for silent)");
                int num9 = num + 20;
                this.cSettings.SilentAimFlick = GUI.Toggle(new Rect(10f, (float) (num = num9), 150f, 20f), this.cSettings.SilentAimFlick, content, style);
                content = new GUIContent("Head");
                bool head = this.cSettings.Head;
                int num10 = num + 20;
                this.cSettings.Head = GUI.Toggle(new Rect(10f, (float) (num = num10), 100f, 20f), this.cSettings.Head, content, style);
                content = new GUIContent("Chest");
                bool chest = this.cSettings.Chest;
                int num11 = num + 20;
                this.cSettings.Chest = GUI.Toggle(new Rect(10f, (float) (num = num11), 100f, 20f), this.cSettings.Chest, content, style);
                if (head && (!chest && this.cSettings.Chest))
                {
                    this.cSettings.Head = false;
                }
                else if (chest && (!head && this.cSettings.Head))
                {
                    this.cSettings.Chest = false;
                }
                content = new GUIContent("crosshair");
                int num12 = num + 20;
                this.cSettings.crosshair = GUI.Toggle(new Rect(10f, (float) (num = num12), 100f, 20f), this.cSettings.crosshair, content, style);
                content = new GUIContent("vis check");
                int num13 = num + 20;
                this.cSettings.VisCheck = GUI.Toggle(new Rect(10f, (float) (num = num13), 100f, 20f), this.cSettings.VisCheck, content, style);
                content = new GUIContent("health check (needs esp health on)");
                int num14 = num + 20;
                this.cSettings.HealthCheck = GUI.Toggle(new Rect(10f, (float) (num = num14), 250f, 20f), this.cSettings.HealthCheck, content, style);
            }
            if (this.tabb == 1)
            {
                content = new GUIContent("Health");
                int num15 = num + 20;
                this.cSettings.showHealth = GUI.Toggle(new Rect(10f, (float) (num = num15), 100f, 20f), this.cSettings.showHealth, content, style);
                content = new GUIContent("2d box");
                int num16 = num + 20;
                this.cSettings.box = GUI.Toggle(new Rect(10f, (float) (num = num16), 100f, 20f), this.cSettings.box, content, style);
                content = new GUIContent("skeleton");
                int num17 = num + 20;
                this.cSettings.skeleton = GUI.Toggle(new Rect(10f, (float) (num = num17), 100f, 20f), this.cSettings.skeleton, content, style);
                content = new GUIContent("snapLines");
                int num18 = num + 20;
                this.cSettings.lines = GUI.Toggle(new Rect(10f, (float) (num = num18), 100f, 20f), this.cSettings.lines, content, style);
                content = new GUIContent("aimLine");
                int num19 = num + 20;
                this.cSettings.aimLine = GUI.Toggle(new Rect(10f, (float) (num = num19), 100f, 20f), this.cSettings.aimLine, content, style);
                content = new GUIContent("gun line");
                int num20 = num + 20;
                this.cSettings.gunLine = GUI.Toggle(new Rect(10f, (float) (num = num20), 100f, 20f), this.cSettings.gunLine, content, style);
            }
            if (this.tabb == 2)
            {
                int num21 = num + 20;
                GUI.Label(new Rect(10f, (float) (num = num21), 150f, 30f), "casual");
                content = new GUIContent("freeCam X");
                int num22 = num + 20;
                this.cSettings.freeCam = GUI.Toggle(new Rect(10f, (float) (num = num22), 100f, 20f), this.cSettings.freeCam, content, style);
                content = new GUIContent("fov changer");
                int num23 = num + 20;
                this.cSettings.fovChanger = GUI.Toggle(new Rect(10f, (float) (num = num23), 100f, 20f), this.cSettings.fovChanger, content, style);
                if (this.cSettings.fovChanger)
                {
                    int num24 = num + 20;
                    this.cSettings.fov = GUI.Slider(new Rect(10f, (float) (num = num24), 100f, 20f), this.cSettings.fov, 1f, 50f, 200f, GUI.skin.horizontalSlider, GUI.skin.horizontalSliderThumb, true, 0, null);
                }
                content = new GUIContent("size changer");
                int num25 = num + 20;
                this.cSettings.sizeChanger = GUI.Toggle(new Rect(10f, (float) (num = num25), 100f, 20f), this.cSettings.sizeChanger, content, style);
                if (this.cSettings.sizeChanger)
                {
                    int num26 = num + 20;
                    this.cSettings.size = GUI.Slider(new Rect(10f, (float) (num = num26), 100f, 20f), this.cSettings.size, 0.01f, 0.01f, 100f, GUI.skin.horizontalSlider, GUI.skin.horizontalSliderThumb, true, 1, null);
                }
                content = new GUIContent("spinbot (fake)");
                int num27 = num + 20;
                this.cSettings.SpinBot = GUI.Toggle(new Rect(10f, (float) (num = num27), 150f, 20f), this.cSettings.SpinBot, content, style);
                content = new GUIContent("spinbot (real)");
                int num28 = num + 20;
                this.cSettings.SpinniBottoni = GUI.Toggle(new Rect(10f, (float) (num = num28), 150f, 20f), this.cSettings.SpinniBottoni, content, style);
                if (this.cSettings.SpinBot || this.cSettings.SpinniBottoni)
                {
                    int num29 = num + 20;
                    this.cSettings.SpinBotSpeed = GUI.Slider(new Rect(10f, (float) (num = num29), 100f, 20f), this.cSettings.SpinBotSpeed, 1f, 100f, 10000f, GUI.skin.horizontalSlider, GUI.skin.horizontalSliderThumb, true, 2, null);
                }
                num += 40;
                content = new GUIContent("chams");
                int num30 = num + 20;
                this.cSettings.chams = GUI.Toggle(new Rect(10f, (float) (num = num30), 100f, 20f), this.cSettings.chams, content, style);
                content = new GUIContent("ignore self");
                int num31 = num + 20;
                this.cSettings.ignoreself = GUI.Toggle(new Rect(10f, (float) (num = num31), 100f, 20f), this.cSettings.ignoreself, content, style);
                num += 40;
                if (GUI.Button(new Rect(10f, (float) num, 20f, 20f), this.navBackButtonTexture))
                {
                    this.cSettings.chamsOption = (this.cSettings.chamsOption <= 0) ? 2 : (this.cSettings.chamsOption - 1);
                }
                GUI.Label(new Rect(15f, (float) (num + 1), 150f, 30f), "<");
                switch (this.cSettings.chamsOption)
                {
                    case 0:
                        GUI.Label(new Rect(35f, (float) (num + 1), 150f, 30f), "Invisible?");
                        break;

                    case 1:
                        GUI.Label(new Rect(35f, (float) (num + 1), 150f, 30f), "Rainbow");
                        break;

                    case 2:
                        GUI.Label(new Rect(35f, (float) (num + 1), 150f, 30f), "Wireframe");
                        break;

                    default:
                        break;
                }
                if (GUI.Button(new Rect(120f, (float) num, 20f, 20f), this.navBackButtonTexture))
                {
                    this.cSettings.chamsOption = (this.cSettings.chamsOption >= 2) ? 0 : (this.cSettings.chamsOption + 1);
                }
                GUI.Label(new Rect(130f, (float) (num + 1), 150f, 30f), ">");
                int num32 = num + 40;
                if (GUI.Button(new Rect(10f, (float) (num = num32), 100f, 20f), this.navBackButtonTexture))
                {
                    this.SaveSettings();
                }
                GUI.Label(new Rect(12f, (float) num, 150f, 30f), "save settings");
                int num33 = 50 + 20;
                GUI.Label(new Rect(200f, (float) (num = num33), 150f, 30f), "player");
                int num34 = num + 20;
                if (GUI.Button(new Rect(200f, (float) (num = num34), 100f, 20f), this.navBackButtonTexture))
                {
                    this.cSettings.tpToClosestChest = true;
                }
                GUI.Label(new Rect(215f, 90f, 150f, 30f), "tp to chest");
                content = new GUIContent("bhop");
                int num35 = num + 20;
                this.cSettings.bhop = GUI.Toggle(new Rect(200f, (float) (num = num35), 100f, 20f), this.cSettings.bhop, content, style);
                content = new GUIContent("killAll K");
                int num36 = num + 20;
                this.cSettings.killAll = GUI.Toggle(new Rect(200f, (float) (num = num36), 100f, 20f), this.cSettings.killAll, content, style);
                content = new GUIContent("crawl");
                int num37 = num + 20;
                this.cSettings.crawl = GUI.Toggle(new Rect(200f, (float) (num = num37), 100f, 20f), this.cSettings.crawl, content, style);
                content = new GUIContent("tpEveryone Y");
                int num38 = num + 20;
                this.cSettings.tpEveryoneOnTopActive = GUI.Toggle(new Rect(200f, (float) (num = num38), 100f, 20f), this.cSettings.tpEveryoneOnTopActive, content, style);
                content = new GUIContent("tp to target Q");
                int num39 = num + 20;
                this.cSettings.tpToTarget = GUI.Toggle(new Rect(200f, (float) (num = num39), 100f, 20f), this.cSettings.tpToTarget, content, style);
                content = new GUIContent("noClip G");
                int num40 = num + 20;
                this.cSettings.noClipActive = GUI.Toggle(new Rect(200f, (float) (num = num40), 100f, 20f), this.cSettings.noClipActive, content, style);
                content = new GUIContent("immortal");
                int num41 = num + 20;
                this.cSettings.immortal = GUI.Toggle(new Rect(200f, (float) (num = num41), 100f, 20f), this.cSettings.immortal, content, style);
                content = new GUIContent("fly T");
                int num42 = num + 20;
                this.cSettings.creative = GUI.Toggle(new Rect(200f, (float) (num = num42), 100f, 20f), this.cSettings.creative, content, style);
                content = new GUIContent("stats modifier");
                int num43 = num + 20;
                this.cSettings.statsMod = GUI.Toggle(new Rect(200f, (float) (num = num43), 100f, 20f), this.cSettings.statsMod, content, style);
                content = new GUIContent("explicit names");
                int num44 = num + 20;
                this.cSettings.customName = GUI.Toggle(new Rect(200f, (float) (num = num44), 100f, 20f), this.cSettings.customName, content, style);
                content = new GUIContent("skin changer");
                int num45 = num + 20;
                this.cSettings.skinChanger = GUI.Toggle(new Rect(200f, (float) (num = num45), 100f, 20f), this.cSettings.skinChanger, content, style);
                int num46 = num + 20;
                this.cSettings.skinId = (int) GUI.Slider(new Rect(200f, (float) (num = num46), 281f, 20f), (float) this.cSettings.skinId, 1f, 1f, 281f, GUI.skin.horizontalSlider, GUI.skin.horizontalSliderThumb, true, 0x4fe830, null);
                int num47 = num + 20;
                GUI.Label(new Rect(200f, (float) (num = num47), 250f, 30f), "id: " + this.cSettings.skinId.ToString() + " name: " + this.nameFromId(this.cSettings.skinId));
                int num48 = 50 + 20;
                GUI.Label(new Rect(360f, (float) (num = num48), 150f, 30f), "weapon");
                content = new GUIContent("one tap walls");
                int num49 = num + 20;
                this.cSettings.oneTapWalls = GUI.Toggle(new Rect(360f, (float) (num = num49), 100f, 20f), this.cSettings.oneTapWalls, content, style);
                content = new GUIContent("infinite Ammo");
                int num50 = num + 20;
                this.cSettings.infiniteAmmo = GUI.Toggle(new Rect(360f, (float) (num = num50), 100f, 20f), this.cSettings.infiniteAmmo, content, style);
                content = new GUIContent("infinite builds");
                int num51 = num + 20;
                this.cSettings.infiniteResource = GUI.Toggle(new Rect(360f, (float) (num = num51), 100f, 20f), this.cSettings.infiniteResource, content, style);
                content = new GUIContent("no spread");
                int num52 = num + 20;
                this.cSettings.noSpread = GUI.Toggle(new Rect(360f, (float) (num = num52), 100f, 20f), this.cSettings.noSpread, content, style);
                content = new GUIContent("RapidFire");
                int num53 = num + 20;
                this.cSettings.rapidFire = GUI.Toggle(new Rect(360f, (float) (num = num53), 100f, 20f), this.cSettings.rapidFire, content, style);
                content = new GUIContent("gunTP ");
                int num54 = num + 20;
                this.cSettings.bulletTp = GUI.Toggle(new Rect(360f, (float) (num = num54), 100f, 20f), this.cSettings.bulletTp, content, style);
                GUI.Label(new Rect(380f, 190f, 100f, 20f), "               buggy   ");
                content = new GUIContent("shoot multiple bullets");
                int num55 = num + 20;
                this.cSettings.shootBig = GUI.Toggle(new Rect(360f, (float) (num = num55), 150f, 20f), this.cSettings.shootBig, content, style);
                int num56 = num + 20;
                GUI.Label(new Rect(360f, (float) (num = num56), 150f, 30f), "(with spread)");
                if (this.cSettings.shootBig)
                {
                    int num57 = num + 20;
                    this.cSettings.Bullets = (int) GUI.Slider(new Rect(360f, (float) (num = num57), 100f, 20f), (float) this.cSettings.Bullets, 1f, 1f, 8000f, GUI.skin.horizontalSlider, GUI.skin.horizontalSliderThumb, true, 0x8771, null);
                    int num58 = num + 20;
                    GUI.Label(new Rect(360f, (float) (num = num58), 150f, 30f), " bullets: " + this.cSettings.Bullets.ToString());
                }
            }
        }

        private string nameFromId(int id) => 
            !this.idToNameMapping.ContainsKey(id) ? "Invalid" : this.idToNameMapping[id];

        public void nameWindow(int windowId)
        {
            int num;
            int num1 = 0 + 20;
            GUI.Button(new Rect(10f, (float) (num = num1), 150f, 20f), this.navBackButtonTexture);
            GUI.Label(new Rect(15f, (float) num, 150f, 30f), "nigger");
            int num2 = num + 20;
            GUI.Button(new Rect(10f, (float) (num = num2), 150f, 20f), this.navBackButtonTexture);
            GUI.Label(new Rect(15f, (float) num, 150f, 30f), "irapeman");
            int num3 = num + 20;
            GUI.Button(new Rect(10f, (float) (num = num3), 150f, 20f), this.navBackButtonTexture);
            GUI.Label(new Rect(15f, (float) num, 150f, 30f), "fuckyounigga");
            int num4 = num + 20;
            GUI.Button(new Rect(10f, (float) (num = num4), 150f, 20f), this.navBackButtonTexture);
            GUI.Label(new Rect(15f, (float) num, 150f, 30f), "fuck");
            int num5 = num + 20;
            GUI.Button(new Rect(10f, (float) (num = num5), 150f, 20f), this.navBackButtonTexture);
            GUI.Label(new Rect(15f, (float) num, 150f, 30f), "cunt");
            int num6 = num + 20;
            GUI.Button(new Rect(10f, (float) (num = num6), 150f, 20f), this.navBackButtonTexture);
            GUI.Label(new Rect(15f, (float) num, 150f, 30f), "big cock");
            int num7 = num + 20;
            GUI.Label(new Rect(15f, (float) (num = num7), 250f, 30f), "youll need to play 1 game");
            int num8 = num + 20;
            GUI.Label(new Rect(15f, (float) (num = num8), 250f, 30f), "for the name to fully updated");
        }

        public  void OnGUI()
        {
            if (this.cSettings.noClip)
            {
                lol.Renderer.DrawString(new Vector2(100f, 150f), "noClip", Color.magenta, true);
            }
            if (this.cSettings.crosshair)
            {
                Drawing.DrawLine(new Vector2((float) ((this.screenWidth / 2) - 5), (float) (this.screenHeight / 2)), new Vector2((float) ((this.screenWidth / 2) + 5), (float) (this.screenHeight / 2)), Color.red, 2f, false);
                Drawing.DrawLine(new Vector2((float) (this.screenWidth / 2), (float) ((this.screenHeight / 2) - 5)), new Vector2((float) (this.screenWidth / 2), (float) ((this.screenHeight / 2) + 5)), Color.red, 2f, false);
            }
            if (this.cSettings.bMenu)
            {
                GUI.backgroundColor = Color.black;
                if (this.setup && (this.logo != null))
                {
                    GUI.DrawTexture(new Rect((float) (((this.screenWidth / 2) - 200) + 0xe1), (float) (((this.screenHeight / 2) - 350) + 0x177), 200f, 200f), this.logo);
                }
                GUI.enabled = true;
                GUI.Window(0, new Rect((float) ((this.screenWidth / 2) - 250), (float) ((this.screenHeight / 2) - 250), 500f, 500f), new GUI.WindowFunction(this.mainWindow), ".gg/sfcommunity  SF CHEATS  menu key: P");
                if (this.cSettings.statsMod)
                {
                    GUI.Window(1, new Rect((float) ((this.screenWidth / 2) + 250), (float) ((this.screenHeight / 2) - 100), 250f, 200f), new GUI.WindowFunction(this.statsWindow), "STATS MODIFIER");
                }
                if (this.cSettings.customName)
                {
                    GUI.Window(2, new Rect((float) ((this.screenWidth / 2) - 500), (float) ((this.screenHeight / 2) - 100), 250f, 200f), new GUI.WindowFunction(this.nameWindow), "SPECIAL NAMES");
                }
                if (!this.cSettings.statsMod && !this.cSettings.customName)
                {
                    GUI.FocusWindow(0);
                }
            }
            if (this.cSettings.useFov && this.cSettings.showFov)
            {
                Drawing.DrawCircle(new Vector2((float) (this.screenWidth / 2), (float) (this.screenHeight / 2)), (int) (this.screenHeight * this.cSettings.fovSize), Color.magenta, 1f, false, 20);
            }
            PlayerController pFMGMMBMDMO = PlayerController.PFMGMMBMDMO;
            float maxValue = float.MaxValue;
            this.closestPlayer2 = Vector3.zero;
            this.closestPlayer3 = Vector3.zero;
            this.closesntHead3 = Vector3.zero;
            if (this.skinnedMeshRenderers != null)
            {
                foreach (SkinnedMeshRenderer renderer in this.skinnedMeshRenderers)
                {
                    Transform[] bones = renderer.bones;
                    Vector3 zero = Vector3.zero;
                    Vector3 playerPos = Vector3.zero;
                    string str = "Head";
                    if (this.cSettings.Chest)
                    {
                        str = "Spine_02";
                    }
                    Transform[] transformArray2 = bones;
                    int index = 0;
                    while (true)
                    {
                        if (index < transformArray2.Length)
                        {
                            Transform transform2 = transformArray2[index];
                            if (transform2.name != str)
                            {
                                index++;
                                continue;
                            }
                            playerPos = transform2.position;
                        }
                        if (playerPos != Vector3.zero)
                        {
                            bool flag = false;
                            Transform bone = this.GetBone(bones, "Hips");
                            if (bone == pFMGMMBMDMO.MFKIGABFACD)
                            {
                                this.mySkinnedMeshRenderer = renderer;
                                flag = true;
                            }
                            bool flag2 = true;
                            if (!flag)
                            {
                                flag2 = IsVisible(playerPos);
                            }
                            Color red = Color.red;
                            if (flag2)
                            {
                                red = new Color(0f, 0.5f, 0f, 1f);
                            }
                            if (this.cSettings.skeleton)
                            {
                                Vector3 position = this.GetBone(bones, "Head").position;
                                Vector3 vector5 = this.GetBone(bones, "Neck").position;
                                Vector3 vector6 = this.GetBone(bones, "Spine_01").position;
                                Vector3 vector7 = this.GetBone(bones, "Spine_02").position;
                                Vector3 vector8 = this.GetBone(bones, "Spine_03").position;
                                Vector3 vector9 = bone.position;
                                Vector3 vector10 = this.GetBone(bones, "Shoulder_R").position;
                                Vector3 vector11 = this.GetBone(bones, "Elbow_R").position;
                                Vector3 vector13 = this.GetBone(bones, "Shoulder_L").position;
                                Vector3 vector14 = this.GetBone(bones, "Elbow_L").position;
                                Vector3 vector16 = this.GetBone(bones, "UpperLeg_R").position;
                                Vector3 vector17 = this.GetBone(bones, "LowerLeg_R").position;
                                Vector3 vector19 = this.GetBone(bones, "UpperLeg_L").position;
                                Vector3 vector20 = this.GetBone(bones, "LowerLeg_L").position;
                                this.drawLineFromTo(position, vector5, red);
                                this.drawLineFromTo(vector5, vector8, red);
                                this.drawLineFromTo(vector8, vector7, red);
                                this.drawLineFromTo(vector7, vector6, red);
                                this.drawLineFromTo(vector6, vector9, red);
                                this.drawLineFromTo(vector5, vector10, red);
                                this.drawLineFromTo(vector10, vector11, red);
                                this.drawLineFromTo(vector11, this.GetBone(bones, "Hand_R").position, red);
                                this.drawLineFromTo(vector5, vector13, red);
                                this.drawLineFromTo(vector13, vector14, red);
                                this.drawLineFromTo(vector14, this.GetBone(bones, "Hand_L").position, red);
                                this.drawLineFromTo(vector9, vector16, red);
                                this.drawLineFromTo(vector16, vector17, red);
                                this.drawLineFromTo(vector17, this.GetBone(bones, "Ankle_R").position, red);
                                this.drawLineFromTo(vector9, vector19, red);
                                this.drawLineFromTo(vector19, vector20, red);
                                this.drawLineFromTo(vector20, this.GetBone(bones, "Ankle_L").position, red);
                            }
                            if (!flag)
                            {
                                if (this.cSettings.tpEveryoneOnTop && (this.cSettings.tpEveryoneOnTopActive && (this.mySkinnedMeshRenderer != null)))
                                {
                                    Vector3 position = this.GetBone(this.mySkinnedMeshRenderer.bones, "Head").position;
                                    float singlePtr1 = position.y;
                                    singlePtr1++;
                                    renderer.rootBone.position = position;
                                }
                                Vector3 pointB = Vector3.zero;
                                if (this.cSettings.box || this.cSettings.lines)
                                {
                                    pointB = this.w2s((this.GetBone(bones, "Ankle_L").position + this.GetBone(bones, "Ankle_R").position) / 2f);
                                }
                                if (this.cSettings.box)
                                {
                                    Vector3 position = this.GetBone(bones, "Head").position;
                                    float singlePtr2 = position.y;
                                    singlePtr2 += 0.25f;
                                    Vector3 vector24 = this.w2s(position);
                                    if (vector24.z <= 0f)
                                    {
                                        break;
                                    }
                                    float num6 = Math.Abs((float) (pointB.y - vector24.y)) * 0.6f;
                                    this.drawBox(new Vector2(vector24.x - (num6 / 2f), vector24.y), new Vector2(pointB.x + (num6 / 2f), pointB.y), red);
                                }
                                float num3 = 0f;
                                PlayerController controller2 = null;
                                if (this.cSettings.showHealth)
                                {
                                    playerControllers = PlayersManager.KODEGOFIJIC.MBFKFLNBNAG;
                                    foreach (PlayerController controller3 in playerControllers.Values)
                                    {
                                        if (bone == controller3.MFKIGABFACD)
                                        {
                                            controller2 = controller3;
                                            Vector3 position = this.GetBone(bones, "Head").position;
                                            float singlePtr3 = position.y;
                                            singlePtr3 += 0.25f;
                                            Vector3 vector26 = this.w2s(position);
                                            float num7 = ((float) controller3.MLCGAAINICC.CAAAJKKKDMK) / 100f;
                                            float num8 = ((float) controller3.MLCGAAINICC.NDILGLLNDAI) / 100f;
                                            float single1 = Math.Abs((float) (pointB.y - vector26.y));
                                            float num9 = single1 * 0.8f;
                                            Vector2 pointA = new Vector2(pointB.x + (num9 / 2f), pointB.y);
                                            float y = pointA.y + (num7 * (vector26.y - pointA.y));
                                            Drawing.DrawLine(pointA, new Vector2(pointA.x, y), new Color(0f, 0.75f, 0f, 1f), 2f, true);
                                            num9 = single1 * 0.7f;
                                            pointA = new Vector2(pointB.x + (num9 / 2f), pointB.y);
                                            float num11 = pointA.y + (num8 * (vector26.y - pointA.y));
                                            Drawing.DrawLine(pointA, new Vector2(pointA.x, num11), Color.blue, 2f, true);
                                        }
                                    }
                                }
                                zero = this.w2s(playerPos);
                                if (zero != Vector3.zero)
                                {
                                    float num4 = Vector2.Distance(new Vector2(zero.x, zero.y), new Vector2((float) (this.screenWidth / 2), (float) (this.screenHeight / 2)));
                                    if (this.cSettings.lines)
                                    {
                                        Drawing.DrawLine(new Vector2((float) (this.screenWidth / 2), (float) this.screenHeight), pointB, red, 1f, true);
                                    }
                                    if (!this.cSettings.HealthCheck || (!this.cSettings.showHealth || (num3 != 0f)))
                                    {
                                        if (this.cSettings.useFov)
                                        {
                                            if (!this.cSettings.VisCheck || this.cSettings.bulletTp)
                                            {
                                                if ((num4 < ((int) (this.screenHeight * this.cSettings.fovSize))) && (num4 < maxValue))
                                                {
                                                    this.closestController = controller2;
                                                    maxValue = num4;
                                                    this.closestPlayer3 = playerPos;
                                                    this.closestPlayer2 = zero;
                                                    this.closesntHead3 = this.GetBone(bones, "Head").position;
                                                }
                                            }
                                            else if (num4 < ((int) (this.screenHeight * this.cSettings.fovSize)))
                                            {
                                                if ((num4 < maxValue) && flag2)
                                                {
                                                    this.closestController = controller2;
                                                    maxValue = num4;
                                                    this.closestPlayer3 = playerPos;
                                                    this.closestPlayer2 = zero;
                                                    this.closesntHead3 = this.GetBone(bones, "Head").position;
                                                }
                                                this.closestController = controller2;
                                            }
                                        }
                                        else if (!this.cSettings.VisCheck || this.cSettings.bulletTp)
                                        {
                                            if (num4 < maxValue)
                                            {
                                                this.closestController = controller2;
                                                maxValue = num4;
                                                this.closestPlayer3 = playerPos;
                                                this.closestPlayer2 = zero;
                                                this.closesntHead3 = this.GetBone(bones, "Head").position;
                                            }
                                        }
                                        else if (num4 < maxValue)
                                        {
                                            if (flag2)
                                            {
                                                this.closestController = controller2;
                                                maxValue = num4;
                                                this.closestPlayer3 = playerPos;
                                                this.closestPlayer2 = zero;
                                                this.closesntHead3 = this.GetBone(bones, "Head").position;
                                            }
                                            this.closestController = controller2;
                                        }
                                    }
                                }
                            }
                        }
                        break;
                    }
                }
            }
            WeaponsController aJEAHIPKNPG = pFMGMMBMDMO.AJEAHIPKNPG;
            if (this.cSettings.aimLine && (this.closestPlayer2 != Vector3.zero))
            {
                Drawing.DrawLine(new Vector2((float) (this.screenWidth / 2), (float) (this.screenHeight / 2)), new Vector2(this.closestPlayer2.x, this.closestPlayer2.y), Color.red, 2f, true);
            }
            if (this.cSettings.gunLine && ((this.closestPlayer2 != Vector3.zero) && ((this.weaponOriginPosition != Vector3.zero) && ((this.currentWeapon != KDNONBOEMLD.None) && ((this.currentWeapon != KDNONBOEMLD.Melee) && (this.currentWeapon != KDNONBOEMLD.Recovery))))))
            {
                Vector3 vector28 = Camera.main.WorldToScreenPoint(this.weaponOriginPosition);
                if (vector28.z > 0f)
                {
                    vector28.y = this.screenHeight - vector28.y;
                    Drawing.DrawLine(new Vector2(vector28.x, vector28.y), new Vector2(this.closestPlayer2.x, this.closestPlayer2.y), Color.blue, 2f, true);
                }
            }
        }

        private void PopulateIdToNameMapping()
        {
            this.idToNameMapping.Add(10, "Rey");
            this.idToNameMapping.Add(0x11, "John");
            this.idToNameMapping.Add(0x12, "Hilda");
            this.idToNameMapping.Add(0x3d, "Annie");
            this.idToNameMapping.Add(0x3e, "Jane");
            this.idToNameMapping.Add(0x3f, "James");
            this.idToNameMapping.Add(0x40, "Liam");
            this.idToNameMapping.Add(70, "Tourist Tanner");
            this.idToNameMapping.Add(0x47, "Tourist Henry");
            this.idToNameMapping.Add(0x48, "Tourist Ellie");
            this.idToNameMapping.Add(0x49, "Tourist Lily");
            this.idToNameMapping.Add(0x4a, "Ava");
            this.idToNameMapping.Add(0x4b, "Harper");
            this.idToNameMapping.Add(0x4c, "Emily");
            this.idToNameMapping.Add(0x4d, "Jenna");
            this.idToNameMapping.Add(0x4e, "Erin");
            this.idToNameMapping.Add(0x4f, "Mason");
            this.idToNameMapping.Add(80, "William");
            this.idToNameMapping.Add(0x51, "Daniel");
            this.idToNameMapping.Add(0x52, "Chloe");
            this.idToNameMapping.Add(0x53, "Ana");
            this.idToNameMapping.Add(0x54, "Charlotte");
            this.idToNameMapping.Add(0x55, "Maya");
            this.idToNameMapping.Add(0x56, "Todd");
            this.idToNameMapping.Add(0x57, "Blake");
            this.idToNameMapping.Add(0x58, "Cooper");
            this.idToNameMapping.Add(0x59, "Officer Bob");
            this.idToNameMapping.Add(90, "Officer Hopper");
            this.idToNameMapping.Add(0x5b, "Officer Mike");
            this.idToNameMapping.Add(0x5c, "Officer Reese");
            this.idToNameMapping.Add(0x5d, "Officer Sarah");
            this.idToNameMapping.Add(0x5e, "Officer Forbes");
            this.idToNameMapping.Add(0x5f, "Constructor Jackson");
            this.idToNameMapping.Add(0x60, "Constructor Jason");
            this.idToNameMapping.Add(0x61, "Constructor Alexis");
            this.idToNameMapping.Add(0x62, "Constructor Molly");
            this.idToNameMapping.Add(0x63, "Grace");
            this.idToNameMapping.Add(100, "Trainer Samantha");
            this.idToNameMapping.Add(0x65, "Trainer Taylor");
            this.idToNameMapping.Add(0x66, "Trainer Ethan");
            this.idToNameMapping.Add(0x67, "Trainer Caleb");
            this.idToNameMapping.Add(0x68, "Maria");
            this.idToNameMapping.Add(0x69, "Amber");
            this.idToNameMapping.Add(0x6a, "Fireman Cayden");
            this.idToNameMapping.Add(0x6b, "Fireman Jack");
            this.idToNameMapping.Add(0x6c, "Tyler");
            this.idToNameMapping.Add(0x6d, "Eric");
            this.idToNameMapping.Add(110, "Cole");
            this.idToNameMapping.Add(0x6f, "Carlos");
            this.idToNameMapping.Add(0x70, "Yue");
            this.idToNameMapping.Add(0x71, "Kayla");
            this.idToNameMapping.Add(0x74, "Wong");
            this.idToNameMapping.Add(0x75, "Agent Maggie");
            this.idToNameMapping.Add(0x76, "Agent Louis");
            this.idToNameMapping.Add(0x7f, "Jenny");
            this.idToNameMapping.Add(0x87, "Punk Connor");
            this.idToNameMapping.Add(0x88, "Adam");
            this.idToNameMapping.Add(0x8a, "Michael");
            this.idToNameMapping.Add(0x9d, "Trainer Madison");
            this.idToNameMapping.Add(0x9e, "Trainer Thomas");
            this.idToNameMapping.Add(1, "Agent Olivia");
            this.idToNameMapping.Add(7, "Caty");
            this.idToNameMapping.Add(8, "Beach Girl");
            this.idToNameMapping.Add(0x15, "Astrid");
            this.idToNameMapping.Add(0x17, "Dre");
            this.idToNameMapping.Add(0x18, "Sonya");
            this.idToNameMapping.Add(0x19, "Jessica");
            this.idToNameMapping.Add(0x1b, "Brandy");
            this.idToNameMapping.Add(0x1d, "Letty");
            this.idToNameMapping.Add(30, "Gisele");
            this.idToNameMapping.Add(0x1f, "Nitro");
            this.idToNameMapping.Add(0x22, "Clay");
            this.idToNameMapping.Add(0x23, "Motor Helmet");
            this.idToNameMapping.Add(0x24, "Traffic Cone");
            this.idToNameMapping.Add(0x25, "Welder's Mask");
            this.idToNameMapping.Add(0x2b, "Cosmo");
            this.idToNameMapping.Add(0x2d, "Santa Hat");
            this.idToNameMapping.Add(0x36, "Rebecca");
            this.idToNameMapping.Add(0x38, "VR");
            this.idToNameMapping.Add(0x39, "Transmission");
            this.idToNameMapping.Add(0x3a, "Stereo");
            this.idToNameMapping.Add(0x72, "Mei");
            this.idToNameMapping.Add(0x73, "Li");
            this.idToNameMapping.Add(0x77, "Agent Sean");
            this.idToNameMapping.Add(120, "Stephanie");
            this.idToNameMapping.Add(0x79, "Caroline");
            this.idToNameMapping.Add(0x7a, "Amy");
            this.idToNameMapping.Add(0x7b, "Fiona");
            this.idToNameMapping.Add(0x7c, "Nova");
            this.idToNameMapping.Add(0x7d, "Skating Boy");
            this.idToNameMapping.Add(0x7e, "Parkour Boy");
            this.idToNameMapping.Add(0x80, "Aubrey");
            this.idToNameMapping.Add(0x81, "Michelle");
            this.idToNameMapping.Add(130, "Robin");
            this.idToNameMapping.Add(0x83, "Sofia");
            this.idToNameMapping.Add(0x84, "Gianna");
            this.idToNameMapping.Add(0x85, "Avery");
            this.idToNameMapping.Add(0x86, "Punk Parker");
            this.idToNameMapping.Add(0x89, "Jose");
            this.idToNameMapping.Add(0x8b, "Owen");
            this.idToNameMapping.Add(140, "Luca");
            this.idToNameMapping.Add(0x8d, "Skater Girl");
            this.idToNameMapping.Add(0x8e, "Amelia");
            this.idToNameMapping.Add(0x8f, "Raelynn");
            this.idToNameMapping.Add(0x90, "Bill Bone");
            this.idToNameMapping.Add(0x91, "Pete Blackbeard");
            this.idToNameMapping.Add(0x92, "Max");
            this.idToNameMapping.Add(0x93, "Victor");
            this.idToNameMapping.Add(0x94, "Carrie");
            this.idToNameMapping.Add(0x95, "Miley");
            this.idToNameMapping.Add(150, "Bernard");
            this.idToNameMapping.Add(0x97, "Teddy");
            this.idToNameMapping.Add(0x98, "Tourist Lucas");
            this.idToNameMapping.Add(0x99, "Tourist Julie");
            this.idToNameMapping.Add(0x9a, "Angela");
            this.idToNameMapping.Add(0x9b, "Sakura");
            this.idToNameMapping.Add(0x9c, "Mary");
            this.idToNameMapping.Add(0x9f, "Fireman Johnson");
            this.idToNameMapping.Add(160, "Elijah");
            this.idToNameMapping.Add(0xa8, "Nathan");
            this.idToNameMapping.Add(0xb9, "Cpt.Saturn");
            this.idToNameMapping.Add(2, "Swat");
            this.idToNameMapping.Add(4, "Skater Boy");
            this.idToNameMapping.Add(5, "Lola");
            this.idToNameMapping.Add(11, "Justin");
            this.idToNameMapping.Add(0x16, "Olaf");
            this.idToNameMapping.Add(0x20, "Rock");
            this.idToNameMapping.Add(0x21, "Mia");
            this.idToNameMapping.Add(0x2c, "Wanda");
            this.idToNameMapping.Add(0x31, "Mal");
            this.idToNameMapping.Add(50, "Red");
            this.idToNameMapping.Add(0x34, "Nebula");
            this.idToNameMapping.Add(60, "Octavia");
            this.idToNameMapping.Add(0xa1, "Master Jeong");
            this.idToNameMapping.Add(0xa2, "Ayaka");
            this.idToNameMapping.Add(0xa3, "Desert Soldier");
            this.idToNameMapping.Add(0xa4, "Jung");
            this.idToNameMapping.Add(0xa5, "Tao");
            this.idToNameMapping.Add(0xa6, "Alice");
            this.idToNameMapping.Add(0xa7, "Ola");
            this.idToNameMapping.Add(0xa9, "Naor");
            this.idToNameMapping.Add(170, "Gnul");
            this.idToNameMapping.Add(0xab, "Crog");
            this.idToNameMapping.Add(0xac, "Alana");
            this.idToNameMapping.Add(0xad, "Mr Arthur");
            this.idToNameMapping.Add(0xae, "Jill");
            this.idToNameMapping.Add(0xaf, "Viper");
            this.idToNameMapping.Add(0xb0, "Emilia");
            this.idToNameMapping.Add(0xb1, "Meera");
            this.idToNameMapping.Add(0xb2, "Ashe");
            this.idToNameMapping.Add(0xb3, "Iris");
            this.idToNameMapping.Add(180, "Athena");
            this.idToNameMapping.Add(0xb5, "Lucy");
            this.idToNameMapping.Add(0xb6, "Cpt.Luna");
            this.idToNameMapping.Add(0xb7, "Cpt.Aria");
            this.idToNameMapping.Add(0xb8, "Cpt.Mars");
            this.idToNameMapping.Add(0xba, "Zoe");
            this.idToNameMapping.Add(0xbb, "DEA Sienna");
            this.idToNameMapping.Add(0xbc, "DEA Julia");
            this.idToNameMapping.Add(0xbd, "DEA Jacob");
            this.idToNameMapping.Add(190, "DEA Diego");
            this.idToNameMapping.Add(0xbf, "Agent Violet");
            this.idToNameMapping.Add(0xc0, "Agent Oliver");
            this.idToNameMapping.Add(0xc1, "Shay");
            this.idToNameMapping.Add(0xc2, "Camilla");
            this.idToNameMapping.Add(0xc3, "Ella");
            this.idToNameMapping.Add(0xc4, "Drift");
            this.idToNameMapping.Add(0xc5, "Sophia");
            this.idToNameMapping.Add(0xc6, "Nash");
            this.idToNameMapping.Add(0xc7, "Sparkles");
            this.idToNameMapping.Add(0xc9, "Rock Man");
            this.idToNameMapping.Add(0xca, "Laz");
            this.idToNameMapping.Add(6, "Ninja Oni");
            this.idToNameMapping.Add(9, "LOL Pump");
            this.idToNameMapping.Add(0x13, "Jester");
            this.idToNameMapping.Add(20, "Executioner");
            this.idToNameMapping.Add(0x27, "Barbarossa");
            this.idToNameMapping.Add(40, "Minerva");
            this.idToNameMapping.Add(0x2a, "Santa");
            this.idToNameMapping.Add(0x33, "Reptile");
            this.idToNameMapping.Add(0x37, "Cayde");
            this.idToNameMapping.Add(0x3b, "X-Droid");
            this.idToNameMapping.Add(200, "Lava Man");
            this.idToNameMapping.Add(0xcb, "Vasse");
            this.idToNameMapping.Add(0xcc, "Sir Romeo");
            this.idToNameMapping.Add(0xcd, "Sir Robert");
            this.idToNameMapping.Add(0xce, "Chemical Sonny");
            this.idToNameMapping.Add(0xcf, "Chemical Colton");
            this.idToNameMapping.Add(0xd0, "Tactical Soldier");
            this.idToNameMapping.Add(0xd1, "Bard");
            this.idToNameMapping.Add(210, "Wizard Adamant");
            this.idToNameMapping.Add(0xd3, "Wizard Silas");
            this.idToNameMapping.Add(0xd4, "Princess Ariela");
            this.idToNameMapping.Add(0xd5, "Princess Eliana");
            this.idToNameMapping.Add(0xd6, "Prince Henry");
            this.idToNameMapping.Add(0xd7, "Prince Edward");
            this.idToNameMapping.Add(0xd8, "Blair");
            this.idToNameMapping.Add(0xd9, "Da Bomb");
            this.idToNameMapping.Add(0xda, "Shani");
            this.idToNameMapping.Add(0xdb, "Akodia");
            this.idToNameMapping.Add(220, "Aegnor");
            this.idToNameMapping.Add(0xdd, "Callon");
            this.idToNameMapping.Add(0xde, "Ninja Hanzo");
            this.idToNameMapping.Add(0xdf, "Ben");
            this.idToNameMapping.Add(0xe0, "Achilles");
            this.idToNameMapping.Add(0xe1, "Beatrix");
            this.idToNameMapping.Add(0xe2, "Alessia");
            this.idToNameMapping.Add(0xe3, "Dana");
            this.idToNameMapping.Add(0xe4, "Sage");
            this.idToNameMapping.Add(0xe5, "Master Fu");
            this.idToNameMapping.Add(230, "Mr.Jones");
            this.idToNameMapping.Add(0xe7, "Madeline");
            this.idToNameMapping.Add(0xe8, "Lulu");
            this.idToNameMapping.Add(0xe9, "Samurai Toyotomi");
            this.idToNameMapping.Add(0xea, "Samurai Miyamoto");
            this.idToNameMapping.Add(0xeb, "Hujuk");
            this.idToNameMapping.Add(0xec, "Haru");
            this.idToNameMapping.Add(0xed, "Kaito");
            this.idToNameMapping.Add(0xee, "Hazmat Blue");
            this.idToNameMapping.Add(0xef, "Oswald");
            this.idToNameMapping.Add(240, "Salvatore");
            this.idToNameMapping.Add(0xfd, "Gemini");
            this.idToNameMapping.Add(0x100, "Eliminator");
            this.idToNameMapping.Add(0x102, "Bun Bun");
            this.idToNameMapping.Add(0x106, "Lady Pop");
            this.idToNameMapping.Add(0x108, "Hel");
            this.idToNameMapping.Add(0x117, "Sunny");
            this.idToNameMapping.Add(3, "X-bot");
            this.idToNameMapping.Add(12, "Hot Dog");
            this.idToNameMapping.Add(0x1a, "Scarecrow");
            this.idToNameMapping.Add(0x1c, "Takashi");
            this.idToNameMapping.Add(0x26, "Witch Doctor");
            this.idToNameMapping.Add(0x2e, "Horseman");
            this.idToNameMapping.Add(0x2f, "Rooster");
            this.idToNameMapping.Add(0x30, "Isaac");
            this.idToNameMapping.Add(0x35, "Bumblebee");
            this.idToNameMapping.Add(0xf1, "Samurai Tokugawa");
            this.idToNameMapping.Add(0xf2, "Ninja Reo");
            this.idToNameMapping.Add(0xf3, "Nitro Bot");
            this.idToNameMapping.Add(0xf4, "Wizard Vasilis");
            this.idToNameMapping.Add(0xf5, "Cpt.Bonnet");
            this.idToNameMapping.Add(0xf6, "LOL King");
            this.idToNameMapping.Add(0xf7, "LOL Queen");
            this.idToNameMapping.Add(0xf8, "Skyler");
            this.idToNameMapping.Add(0xf9, "Lior");
            this.idToNameMapping.Add(250, "Hazmat Gold");
            this.idToNameMapping.Add(0xfb, "Cicero");
            this.idToNameMapping.Add(0xfc, "Astro");
            this.idToNameMapping.Add(0xfe, "Rainbow");
            this.idToNameMapping.Add(0xff, "Storm");
            this.idToNameMapping.Add(0x113, "Thumper");
            this.idToNameMapping.Add(0x103, "Cyber Bunny");
            this.idToNameMapping.Add(260, "Disco Boy");
            this.idToNameMapping.Add(0x105, "Rocker");
            this.idToNameMapping.Add(0x107, "Zeus");
            this.idToNameMapping.Add(0x109, "Anubis");
            this.idToNameMapping.Add(0x110, "Golden BEN KEYSAR");
            this.idToNameMapping.Add(0x111, "Golden Inde Game");
            this.idToNameMapping.Add(0x112, "Golden MasterOhad");
            this.idToNameMapping.Add(0x114, "Golden Cycnic");
            this.idToNameMapping.Add(0x115, "Golden RonenGG");
            this.idToNameMapping.Add(0x116, "Scooba");
            this.idToNameMapping.Add(0x41, "The Caesar");
            this.idToNameMapping.Add(0x42, "Tron");
            this.idToNameMapping.Add(0x43, "Magma");
            this.idToNameMapping.Add(0x44, "Green Stone");
            this.idToNameMapping.Add(0x45, "Shadow");
            this.idToNameMapping.Add(0x10a, "BEN KEYSAR");
            this.idToNameMapping.Add(0x10b, "Inde Game");
            this.idToNameMapping.Add(0x10c, "MasterOhad");
            this.idToNameMapping.Add(0x10d, "Rainbow Queen");
            this.idToNameMapping.Add(270, "Cycnic");
            this.idToNameMapping.Add(0x10f, "RonenGG");
            this.idToNameMapping.Add(0x119, "The Captain");
        }

        private void SaveSettings()
        {
            string contents = JsonUtility.ToJson(this.cSettings);
            File.WriteAllText(Path.Combine(Application.persistentDataPath, "settings.json"), contents);
        }

        public void Start()
        {
            this.tempFilePath = Path.Combine(Application.persistentDataPath, "coolAssets");
            File.WriteAllBytes(this.tempFilePath, bytes.Assets);
            this.LoadAssetBundleAsync2();
            Drawing.Initialize();
            Loader.Load.SetActive(true);
            this.thisObject = FindObjectOfType<Main>();
            this.thisObject.enabled = true;
            this.PopulateIdToNameMapping();
            this.LoadSettings();
        }

        public void statsWindow(int windowId)
        {
            int num = 0;
            int num1 = num + 20;
            this.cSettings.generalspeed = GUI.Slider(new Rect(10f, (float) (num = num1), 100f, 20f), this.cSettings.generalspeed, 0.1f, 1f, 100f, GUI.skin.horizontalSlider, GUI.skin.horizontalSliderThumb, true, 0x7d4d9, null);
            GUI.Label(new Rect(115f, (float) (num - 4), 150f, 30f), "generalSpeed: " + $"{this.cSettings.generalspeed:N1}");
            int num2 = num + 20;
            this.cSettings.jumpHeight = GUI.Slider(new Rect(10f, (float) (num = num2), 100f, 20f), this.cSettings.jumpHeight, 0.1f, 1f, 100f, GUI.skin.horizontalSlider, GUI.skin.horizontalSliderThumb, true, 0x5ac, null);
            GUI.Label(new Rect(115f, (float) (num - 4), 150f, 30f), "jumpHeight: " + $"{this.cSettings.jumpHeight:N1}");
            int num3 = num + 20;
            this.cSettings.flySpeed = GUI.Slider(new Rect(10f, (float) (num = num3), 100f, 20f), this.cSettings.flySpeed, 0.1f, 1f, 100f, GUI.skin.horizontalSlider, GUI.skin.horizontalSliderThumb, true, 0xb127, null);
            GUI.Label(new Rect(115f, (float) (num - 4), 150f, 30f), "flySpeed: " + $"{this.cSettings.flySpeed:N1}");
            int num4 = num + 20;
            this.cSettings.crawlSpeed = GUI.Slider(new Rect(10f, (float) (num = num4), 100f, 20f), this.cSettings.crawlSpeed, 0.1f, 1f, 100f, GUI.skin.horizontalSlider, GUI.skin.horizontalSliderThumb, true, 0xbb2c, null);
            GUI.Label(new Rect(115f, (float) (num - 4), 150f, 30f), "crawlSpeed: " + $"{this.cSettings.crawlSpeed:N1}");
            int num5 = num + 20;
            GUI.Label(new Rect(10f, (float) (num = num5), 250f, 30f), "to reset stats turn off stats modifier");
            int num6 = num + 10;
            GUI.Label(new Rect(10f, (float) (num = num6), 250f, 30f), "and wait for the next game");
        }

        protected  void Update()
        {
            List<Collider>.Enumerator enumerator;
            Vector3 vector2;
            if (!this.setup && (Shader.Count == 5))
            {
                _textFont = Fonts["Leto_Text_Sans_Defect"];
                _Chams = Shader["Chams"];
                _ColorChams = Shader["ColorChams"];
                _Pulsing = Shader["Pulsing"];
                _Rainbow = Shader["Rainbow"];
                _Wireframe = Shader["Wireframe"];
                this.logo = Base64ToTexture(bytes.logo);
                this.setup = true;
            }
            if (Input.GetKeyDown(KeyCode.P))
            {
                this.cSettings.bMenu = !this.cSettings.bMenu;
            }
            if (this.cSettings.noClipActive && Input.GetKeyDown(KeyCode.G))
            {
                this.cSettings.noClip = !this.cSettings.noClip;
            }
            if (this.cSettings.tpEveryoneOnTopActive && Input.GetKeyDown(KeyCode.Y))
            {
                this.cSettings.tpEveryoneOnTop = !this.cSettings.tpEveryoneOnTop;
            }
            if (this.cSettings.creative && Input.GetKeyDown(KeyCode.T))
            {
                this.cSettings.fly = !this.cSettings.fly;
            }
            if (this.cSettings.freeCam && Input.GetKeyDown(KeyCode.X))
            {
                this.inFreeCamMode = !this.inFreeCamMode;
            }
            if (this.cSettings.fovChanger)
            {
                CameraManager.KODEGOFIJIC.MainCamera.fieldOfView = this.cSettings.fov;
            }
            PlayerController pFMGMMBMDMO = PlayerController.PFMGMMBMDMO;
            if (this.cSettings.bhop && Input.GetKey(KeyCode.Space))
            {
                pFMGMMBMDMO.HNADBHINEPA.Jump();
            }
            if (this.cSettings.sizeChanger && PlayerController.PFMGMMBMDMO != null)
            {
                pFMGMMBMDMO.MFKIGABFACD.localScale = new Vector3(this.cSettings.size, this.cSettings.size, this.cSettings.size);
            }
            if ((pFMGMMBMDMO != null) && this.cSettings.creative)
            {
                pFMGMMBMDMO.SetGodMode(this.cSettings.fly);
            }
            if (this.cSettings.statsMod)
            {
                if (!this.resetSpeed)
                {
                    this.defaultSpeed = pFMGMMBMDMO.HNADBHINEPA.speed;
                }
                this.resetSpeed = true;
                pFMGMMBMDMO.HNADBHINEPA.jumpHeight = this.cSettings.jumpHeight;
                pFMGMMBMDMO.HNADBHINEPA.crawlSpeed = this.cSettings.crawlSpeed;
                pFMGMMBMDMO.HNADBHINEPA.flySpeed = this.cSettings.flySpeed;
                pFMGMMBMDMO.HNADBHINEPA.speed = this.cSettings.generalspeed;
                pFMGMMBMDMO.HNADBHINEPA.freeRunningSpeed = this.cSettings.generalspeed;
                pFMGMMBMDMO.HNADBHINEPA.strafeRunningSpeed = this.cSettings.generalspeed;
                pFMGMMBMDMO.HNADBHINEPA.freeRotationSpeed = this.cSettings.generalspeed;
                pFMGMMBMDMO.HNADBHINEPA.strafeRotationSpeed = this.cSettings.generalspeed;
                pFMGMMBMDMO.HNADBHINEPA.freeSprintSpeed = this.cSettings.generalspeed;
                pFMGMMBMDMO.HNADBHINEPA.strafeSprintSpeed = this.cSettings.generalspeed;
                pFMGMMBMDMO.HNADBHINEPA.freeWalkSpeed = this.cSettings.generalspeed;
                pFMGMMBMDMO.HNADBHINEPA.strafeWalkSpeed = this.cSettings.generalspeed;
            }
            PlayerHealth mLCGAAINICC = pFMGMMBMDMO.MLCGAAINICC;
            if (mLCGAAINICC != null)
            {
                mLCGAAINICC.SetPlayerImmunity(this.cSettings.immortal);
            }
            if (this.cSettings.SilentAim && !this.inFreeCamMode)
            {
                WeaponsController aJEAHIPKNPG = pFMGMMBMDMO.AJEAHIPKNPG;
                if (aJEAHIPKNPG != null)
                {
                    WeaponModel model2 = aJEAHIPKNPG.CAJMLMBFGKK;
                    if (model2 != null)
                    {
                        WeaponStats stats2 = model2.LAHNNIPGEFI;
                        if ((this.closestPlayer3 != Vector3.zero) && ((stats2.WeaponType != KDNONBOEMLD.None) && ((stats2.WeaponType != KDNONBOEMLD.Melee) && (stats2.WeaponType != KDNONBOEMLD.Recovery))))
                        {
                            if (this.cSettings.SilentAimFlick)
                            {
                                if (Input.GetMouseButton(0) && (UnityEngine.Random.Range((float) 0f, (float) 100f) <= this.cSettings.hitChance))
                                {
                                    CameraManager.KODEGOFIJIC.TPCamera.gimmiCameraThx().transform.LookAt(this.closestPlayer3);
                                }
                            }
                            else if (UnityEngine.Random.Range((float) 0f, (float) 100f) <= this.cSettings.hitChance)
                            {
                                CameraManager.KODEGOFIJIC.TPCamera.gimmiCameraThx().transform.LookAt(this.closestPlayer3);
                            }
                        }
                    }
                }
            }
            if (this.cSettings.oldCrawl != this.cSettings.crawl)
            {
                pFMGMMBMDMO.HNADBHINEPA.SetCrawling(this.cSettings.crawl);
                this.cSettings.oldCrawl = this.cSettings.crawl;
            }
            this.skinnedMeshRenderers = FindObjectsOfType<SkinnedMeshRenderer>();
            float maxValue = float.MaxValue;
            Transform transform = null;
            foreach (SkinnedMeshRenderer renderer in this.skinnedMeshRenderers)
            {
                Transform[] bones = renderer.bones;
                Transform bone = this.GetBone(bones, "Armature");
                if (bone != null)
                {
                    float num3 = Vector3.Distance(pFMGMMBMDMO.MFKIGABFACD.position, bone.position);
                    if (num3 < maxValue)
                    {
                        maxValue = num3;
                        transform = bone;
                    }
                }
                bool flag = true;
                if (this.cSettings.ignoreself && (this.GetBone(bones, "Hips") == pFMGMMBMDMO.MFKIGABFACD))
                {
                    flag = false;
                }
                if (!this.cSettings.chams)
                {
                    flag = false;
                }
                if (!flag)
                {
                    switch (this.cSettings.chamsOption)
                    {
                        case 0:
                            if (renderer.material.shader == _Chams)
                            {
                                renderer.material.shader = oldShaders[renderer.material.name];
                            }
                            break;

                        case 1:
                            if (renderer.material.shader == _Rainbow)
                            {
                                renderer.material.shader = oldShaders[renderer.material.name];
                            }
                            break;

                        case 2:
                            if (renderer.material.shader == _Wireframe)
                            {
                                renderer.material.shader = oldShaders[renderer.material.name];
                            }
                            break;

                        default:
                            break;
                    }
                }
                else
                {
                    if (!oldShaders.ContainsKey(renderer.material.name))
                    {
                        oldShaders.Add(renderer.material.name, renderer.material.shader);
                    }
                    switch (this.cSettings.chamsOption)
                    {
                        case 0:
                            if ((renderer.material.shader != _Chams) && this.setup)
                            {
                                renderer.material.shader = _Chams;
                                renderer.material.SetColor("_ColorVisible", new Color(1f, 4f, 1f, 0f));
                                renderer.material.SetColor("_ColorBehind", new Color(4f, 1f, 1f, 0f));
                            }
                            break;

                        case 1:
                            if ((renderer.material.shader != _Rainbow) && this.setup)
                            {
                                renderer.material.shader = _Rainbow;
                            }
                            break;

                        case 2:
                            if ((renderer.material.shader != _Wireframe) && this.setup)
                            {
                                renderer.material.shader = _Wireframe;
                                renderer.material.SetColor("_WireColor", new Color(0.5f, 0f, 0.5f, 0.5f));
                            }
                            break;

                        default:
                            break;
                    }
                }
            }
            this.closestChest = transform;
            if (this.cSettings.noClip && this.cSettings.noClipActive)
            {
                if (this._currCollider == 0f)
                {
                    this._currCollider = pFMGMMBMDMO.HNADBHINEPA._currCollider.radius;
                    this._capsuleCollider = pFMGMMBMDMO.HNADBHINEPA._capsuleCollider.radius;
                    this._crawlCollider = pFMGMMBMDMO.HNADBHINEPA._crawlCollider.radius;
                    this._legsCollider = pFMGMMBMDMO.HNADBHINEPA._legsCollider.radius;
                    this.PlayerMainCollider = pFMGMMBMDMO.EJFJHEEGJIK.PlayerMainCollider.radius;
                }
                pFMGMMBMDMO.HNADBHINEPA._currCollider.radius = float.NegativeInfinity;
                pFMGMMBMDMO.HNADBHINEPA._capsuleCollider.radius = float.NegativeInfinity;
                pFMGMMBMDMO.HNADBHINEPA._crawlCollider.radius = float.NegativeInfinity;
                pFMGMMBMDMO.HNADBHINEPA._legsCollider.radius = float.NegativeInfinity;
                pFMGMMBMDMO.EJFJHEEGJIK.PlayerMainCollider.radius = float.NegativeInfinity;
                using (enumerator = pFMGMMBMDMO.EJFJHEEGJIK.EJEDNAEDMNF.PlayerColliders.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        enumerator.Current.enabled = false;
                    }
                }
            }
            else if (this._currCollider != 0f)
            {
                pFMGMMBMDMO.HNADBHINEPA._currCollider.radius = this._currCollider;
                pFMGMMBMDMO.HNADBHINEPA._capsuleCollider.radius = this._capsuleCollider;
                pFMGMMBMDMO.HNADBHINEPA._crawlCollider.radius = this._crawlCollider;
                pFMGMMBMDMO.HNADBHINEPA._legsCollider.radius = this._legsCollider;
                pFMGMMBMDMO.EJFJHEEGJIK.PlayerMainCollider.radius = this.PlayerMainCollider;
                using (enumerator = pFMGMMBMDMO.EJFJHEEGJIK.EJEDNAEDMNF.PlayerColliders.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        enumerator.Current.enabled = true;
                    }
                }
                this._currCollider = 0f;
                this._capsuleCollider = 0f;
                this._crawlCollider = 0f;
                this._legsCollider = 0f;
                this.PlayerMainCollider = 0f;
            }
            if (this.cSettings.tpToTarget && (Input.GetKey(KeyCode.Q) && (this.closesntHead3 != Vector3.zero)))
            {
                CameraManager.KODEGOFIJIC.TPCamera.currentTarget.position = this.closesntHead3;
            }
            CameraManager kODEGOFIJIC = CameraManager.KODEGOFIJIC;
            if (this.cSettings.tpToClosestChest)
            {
                if (this.closestChest != null)
                {
                    kODEGOFIJIC.TPCamera.currentTarget.position = this.closestChest.position;
                }
                this.cSettings.tpToClosestChest = false;
            }
            if (!this.cSettings.SpinBot)
            {
                if (this.cSettings.OldSpinBot)
                {
                    this.currentRotation = kODEGOFIJIC.TPCamera.target.rotation;
                    int index = 0;
                    while (true)
                    {
                        if (index >= kODEGOFIJIC.TPCamera.target.childCount)
                        {
                            this.cSettings.OldSpinBot = false;
                            break;
                        }
                        kODEGOFIJIC.TPCamera.target.GetChild(index).rotation = this.currentRotation;
                        index++;
                    }
                }
            }
            else
            {
                if (!this.gotFirstRotation)
                {
                    this.currentRotation = kODEGOFIJIC.TPCamera.target.parent.root.rotation;
                    this.gotFirstRotation = true;
                }
                Quaternion quaternion = Quaternion.Euler(0f, this.cSettings.SpinBotSpeed * Time.deltaTime, 0f) * this.currentRotation;
                int index = 0;
                while (true)
                {
                    if (index >= kODEGOFIJIC.TPCamera.target.childCount)
                    {
                        this.currentRotation = quaternion;
                        this.cSettings.OldSpinBot = true;
                        break;
                    }
                    kODEGOFIJIC.TPCamera.target.GetChild(index).rotation = quaternion;
                    index++;
                }
            }
            if (!this.cSettings.SpinniBottoni)
            {
                if (this.cSettings.OldSpinniBottoni)
                {
                    this.currentRotation = pFMGMMBMDMO.transform.rotation;
                    pFMGMMBMDMO.transform.rotation = this.currentRotation;
                    this.cSettings.OldSpinniBottoni = false;
                }
            }
            else
            {
                if (!this.gotFirstRotation)
                {
                    this.currentRotation = pFMGMMBMDMO.transform.rotation;
                    this.gotFirstRotation = true;
                }
                Quaternion quaternion2 = Quaternion.Euler(0f, this.cSettings.SpinBotSpeed * Time.deltaTime, 0f) * this.currentRotation;
                pFMGMMBMDMO.transform.rotation = quaternion2;
                this.currentRotation = quaternion2;
                this.cSettings.OldSpinniBottoni = true;
            }
            WeaponModel cAJMLMBFGKK = pFMGMMBMDMO.AJEAHIPKNPG.CAJMLMBFGKK;
            this.weaponOriginPosition = cAJMLMBFGKK.FGADLIIIJCK.position;
            if (this.cSettings.bulletTp && (!this.inFreeCamMode && ((this.closestPlayer3 != Vector3.zero) && ((this.currentWeapon != KDNONBOEMLD.None) && ((this.currentWeapon != KDNONBOEMLD.Melee) && (this.currentWeapon != KDNONBOEMLD.Recovery))))))
            {
                Vector3 vector = this.closestPlayer3;
                float singlePtr1 = vector.y;
                singlePtr1 += 0.5f;
                vector2 = this.closestPlayer3 - vector;
                Quaternion quaternion3 = Quaternion.LookRotation(vector2.normalized);
                cAJMLMBFGKK.FGADLIIIJCK.position = vector;
                cAJMLMBFGKK.FGADLIIIJCK.rotation = quaternion3;
                cAJMLMBFGKK.transform.position = vector;
                cAJMLMBFGKK.transform.rotation = quaternion3;
                CameraManager.KODEGOFIJIC.transform.position = vector;
                CameraManager.KODEGOFIJIC.TPCamera.transform.rotation = quaternion3;
                CameraManager.KODEGOFIJIC.transform.rotation = quaternion3;
            }
            if (this.cSettings.rapidFire && Input.GetMouseButton(0))
            {
                WeaponsController aJEAHIPKNPG = pFMGMMBMDMO.AJEAHIPKNPG;
                if (aJEAHIPKNPG != null)
                {
                    WeaponModel model3 = aJEAHIPKNPG.CAJMLMBFGKK;
                    if (model3 != null)
                    {
                        WeaponStats stats3 = model3.LAHNNIPGEFI;
                        if ((stats3.WeaponType != KDNONBOEMLD.None) && (stats3.WeaponType != KDNONBOEMLD.Recovery))
                        {
                            vector2 = new Vector3();
                            vector2 = new Vector3();
                            vector2 = new Vector3();
                            model3.Fire(vector2, vector2, vector2, false, 0.0);
                        }
                    }
                }
            }
            if (this.cSettings.killAll && (Input.GetKey(KeyCode.K) || this.needToKIll))
            {
                this.needToKIll = true;
                playerControllers = PlayersManager.KODEGOFIJIC.MBFKFLNBNAG;
                if (playerControllers.Count < 2)
                {
                    this.needToKIll = false;
                }
                foreach (PlayerController controller4 in playerControllers.Values)
                {
                    if (controller4 != pFMGMMBMDMO)
                    {
                        Vector3 position = controller4.MFKIGABFACD.position;
                        float singlePtr2 = position.y;
                        singlePtr2 -= 1000f;
                        controller4.TakeDamage("FallDamage", 500, FEJECJGFFJL.Fall, new Vector3?(position));
                    }
                }
            }
            if (this.cSettings.infiniteResource)
            {
                pFMGMMBMDMO.PlayerBuildingManager.AddBuildingAmmo(10);
            }
            if (this.cSettings.infiniteAmmo)
            {
                cAJMLMBFGKK.SetCurrentMagazineAmount(0xf423f);
            }
            WeaponStats lAHNNIPGEFI = cAJMLMBFGKK.LAHNNIPGEFI;
            this.currentWeapon = lAHNNIPGEFI.WeaponType;
            if (this.cSettings.shootBig)
            {
                lAHNNIPGEFI.RoundsPerShot = this.cSettings.Bullets;
                lAHNNIPGEFI.RangeDescription = JODECENBOHM.VeryFar;
                lAHNNIPGEFI.FireRateDescription = MKPFIBGLDEI.VeryFast;
                lAHNNIPGEFI.StatsForLevel.BurstFireDelay = 0f;
                lAHNNIPGEFI.StatsForLevel.RecoilDuration = 0f;
                lAHNNIPGEFI.StatsForLevel.RecoilForce = 0f;
                lAHNNIPGEFI.StatsForLevel.RecoilReturnForce = 0f;
                lAHNNIPGEFI.StatsForLevel.DamageSettings.IsDamageAffectedByDistance = false;
                lAHNNIPGEFI.StatsForLevel.SpreadSettings.DoShotsAlwaysSpread = true;
                lAHNNIPGEFI.StatsForLevel.SpreadSettings.DefaultSpread = 0.1f;
                lAHNNIPGEFI.StatsForLevel.SpreadSettings.AimingSpread = 0.1f;
                lAHNNIPGEFI.StatsForLevel.SpreadSettings.IncreasePerShot = 0f;
                lAHNNIPGEFI.StatsForLevel.SpreadSettings.SpreadOutTime = 0f;
            }
            else
            {
                lAHNNIPGEFI.RangeDescription = JODECENBOHM.VeryFar;
                lAHNNIPGEFI.FireRateDescription = MKPFIBGLDEI.VeryFast;
                lAHNNIPGEFI.StatsForLevel.BurstFireDelay = 0f;
                lAHNNIPGEFI.StatsForLevel.RecoilDuration = 0f;
                lAHNNIPGEFI.StatsForLevel.RecoilForce = 0f;
                lAHNNIPGEFI.StatsForLevel.RecoilReturnForce = 0f;
                lAHNNIPGEFI.StatsForLevel.DamageSettings.IsDamageAffectedByDistance = false;
                lAHNNIPGEFI.StatsForLevel.SpreadSettings.DoShotsAlwaysSpread = true;
                if (this.cSettings.noSpread)
                {
                    lAHNNIPGEFI.StatsForLevel.SpreadSettings.DefaultSpread = 0f;
                    lAHNNIPGEFI.StatsForLevel.SpreadSettings.AimingSpread = 0f;
                    lAHNNIPGEFI.StatsForLevel.SpreadSettings.IncreasePerShot = 0f;
                    lAHNNIPGEFI.StatsForLevel.SpreadSettings.SpreadOutTime = 0f;
                }
            }
            if (this.cSettings.oneTapWalls)
            {
                lAHNNIPGEFI.StatsForLevel.DamageSettings.DamageToBuildings = 1000f;
            }
            cAJMLMBFGKK.LAHNNIPGEFI = lAHNNIPGEFI;
        }

        private Vector3 w2s(Vector3 pos)
        {
            Vector3 vector = Camera.main.WorldToScreenPoint(pos);
            if (vector.z <= 0f)
            {
                return Vector3.zero;
            }
            vector.y = this.screenHeight - vector.y;
            return vector;
        }

        private IEnumerator WaitForAssetLoad(AssetBundleRequest assetRequest, TaskCompletionSource<UnityEngine.Object[]> assetLoadCompletionSource)
        {
            return null;
        }
    }
}

