using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKBody : MonoBehaviour
{

    [SerializeField] private float stepSpeed = 5;
    [SerializeField] float stepForce = 0.1f;
    [SerializeField] float stepLenght = 2f;
    [SerializeField] float stepHeight = 1f;

    [System.Serializable]
    public struct LegList {
        public List<IKFootSolver> legs;
    }

    [SerializeField] List<LegList> legsGroup = new List<LegList>();
    // Start is called before the first frame update
    private int currentGroup = 0;
    private float t = 1;
    void Start() {
        foreach(LegList group in legsGroup) {
            foreach(IKFootSolver leg in group.legs) {
                leg.Init(stepSpeed, stepForce, stepLenght, stepHeight);
            }   
        }
    }

    // Update is called once per frame
    void Update()
    {
        t -= Time.deltaTime * stepSpeed;

        if (t < 0) {
            t = 1;
            foreach(IKFootSolver leg in legsGroup[currentGroup].legs) {
                leg.Step();
            }
            currentGroup++;
            currentGroup %= legsGroup.Count;
        }
    }
}
