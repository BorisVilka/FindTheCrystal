
using UnityEngine;
using UnityEngine.EventSystems;

public class Item : MonoBehaviour, IPointerDownHandler
{
    public Game script { private get; set; }

    private Color[] colors = new Color[]{
        Color.cyan,Color.blue,Color.red,Color.green,Color.magenta,Color.yellow
    };

    private Vector2 size;
    public int ind { get; set; }
    public bool canTouch { get; set; }

    public void OnPointerDown(PointerEventData eventData)
    {
       if(canTouch)  script.add(gameObject);
       //Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        canTouch = true;
       // size GetComponent<MeshRenderer>().bounds.size * 2;
        //  gameObject.GetComponent<Renderer>().material = material;
        // material.color = colors[Random.Range(0, colors.Length)];
    }

    // Update is called once per frame
    void Update()
    {
        //BoxCollider2D collider2D = GetComponent<BoxCollider2D>();
        CapsuleCollider2D capsuleCollider= gameObject.GetComponent<CapsuleCollider2D>();
        capsuleCollider.size = GetComponent<MeshRenderer>().bounds.size*2;
        capsuleCollider.offset = Vector2.zero;
    }
}
