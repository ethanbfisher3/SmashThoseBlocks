using UnityEngine.SceneManagement;
using UnityEngine;
using Utils;
using System.Collections;
using Blocks;
using GameManagement;
using Levels;
using System.Linq;

public class TrajectoryLine : MonoBehaviour
{
    public Scene SimulationScene { get; private set; }

    private PhysicsScene2D PhysicsScene { get; set; }
    [SerializeField]
    private LineRenderer lineRenderer;
    private float LastSceneCreation { get; set; }
    private int simulationNumber;

    public static TrajectoryLine Instance { get; private set; }
    public int maxPhysicsFrameIterations = 10;

    void Awake()
    {
        Instance = this;
        simulationNumber = 0;
        LastSceneCreation = 0;
    }

    void Update()
    {
        var existsSelectedBlock = GameLevel.Current?.SelectedBlock != null;

        if (!existsSelectedBlock || !GameManager.Instance.Playing || GameManager.Instance.IsAboutToWin)
        {
            lineRenderer.enabled = false;
            return;
        }

        var selectedBlock = GameLevel.Current.SelectedBlock;

        if (selectedBlock.IsMoving())
        {
            lineRenderer.enabled = false;
        }
        else
        {
            lineRenderer.enabled = true;
            lineRenderer.transform.position = selectedBlock.transform.position;

            var blockToUse = BlockConstants.Instance.GetBlockPrefab(selectedBlock.gameObject.name.StartsWith("Stick") ?
                    "FlaggedBlock" : selectedBlock.gameObject.name.Substring(0, 5));
            SimulateTrajectory(blockToUse, selectedBlock.transform.position);
        }
    }

    void FixedUpdate()
    {
        if (!GameLevel.Current.shouldRecalculatePhysicsScene) return;

        LastSceneCreation += Time.fixedDeltaTime;
        if (LastSceneCreation > GameLevel.Current.physicsRecalculationTime)
        {
            CreatePhysicsScene();
            LastSceneCreation -= GameLevel.Current.physicsRecalculationTime;
        }
    }

    public void CreatePhysicsScene()
    {
        var oldScene = SimulationScene;

        SimulationScene = SceneManager.CreateScene("Simulation" + simulationNumber, new CreateSceneParameters(LocalPhysicsMode.Physics2D));
        PhysicsScene = SimulationScene.GetPhysicsScene2D();
        simulationNumber++;

        var env = GameLevel.Current.environment;
        foreach (Transform obj in env.GetComponentsInChildren<Transform>())
        {

            var ghostObj = Instantiate(obj.gameObject, obj.position, obj.rotation);
            var renderer = ghostObj.GetComponent<Renderer>();
            if (renderer)
                renderer.enabled = false;

            foreach (Transform child in ghostObj.GetComponentsInChildren<Transform>())
            {
                var renderer1 = child.GetComponent<Renderer>();
                if (renderer1)
                    renderer1.enabled = false;
            }

            SceneManager.MoveGameObjectToScene(ghostObj, SimulationScene);
        }

        if (oldScene.name != null)
            SceneManager.UnloadSceneAsync(oldScene);
    }

    public void SimulateTrajectory(Block blockPrefab, Vector3 position)
    {
        PhysicsScene = SimulationScene.GetPhysicsScene2D();

        var ghostObj = Instantiate(blockPrefab, position, Quaternion.identity);
        ghostObj.GetComponent<Renderer>().enabled = false;
        ghostObj.tag = "Ghost";
        //Destroy(ghostObj.GetComponent<BoxCollider2D>());
        SceneManager.MoveGameObjectToScene(ghostObj.gameObject, SimulationScene);

        ghostObj.ForceThrow(false);

        lineRenderer.positionCount = maxPhysicsFrameIterations;

        for (int i = 0; i < maxPhysicsFrameIterations; i++)
        {
            PhysicsScene.Simulate(Time.fixedDeltaTime);
            lineRenderer.SetPosition(i, ghostObj.transform.position);
        }

        Destroy(ghostObj.gameObject);
    }

    public GameObject GetSimulationGameObject(string name)
    {
        var rootObjects = SimulationScene.GetRootGameObjects();
        foreach (var obj in rootObjects)
        {
            if (obj.name.StartsWith(name))
                return obj;
        }

        return null;
    }

    public void AddObjectToSimulationScene(GameObject gameObject)
    {
        SceneManager.MoveGameObjectToScene(gameObject, SimulationScene);
    }
}
