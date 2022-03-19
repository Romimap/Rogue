using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKBody : MonoBehaviour
{

    [SerializeField] private float stepSpeed = 5;
    [SerializeField] float stepForce = 0.1f;
    [SerializeField] float stepLenght = 2f;
    [SerializeField] float stepHeight = 1f;
    [SerializeField] float bodyHeight = 1.5f;
    [SerializeField] Transform target;

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
    void Update() {
        Vector3 p = target.position;
        float meany = 0;
        float n = 0;
         foreach(LegList group in legsGroup) {
            foreach(IKFootSolver leg in group.legs) {
                meany += leg.transform.position.y;
                n++;
            }   
        }
        meany /= n;
        p.y = meany + bodyHeight;
        transform.position = p;
        transform.rotation = target.rotation;


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
