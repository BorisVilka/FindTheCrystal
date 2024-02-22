using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game : MonoBehaviour
{


    [SerializeField] Material material1;
    [SerializeField] Material material2;
    [SerializeField] Material material3;
    [SerializeField] Material material4;
    [SerializeField] Material material5;
    [SerializeField] Material material6;
    [SerializeField] Material material7;


    [SerializeField] GameObject prefab1;
    [SerializeField] GameObject prefab3;
    [SerializeField] GameObject prefab5;
    [SerializeField] GameObject prefab7;
    [SerializeField] GameObject prefab8;
    [SerializeField] GameObject prefab9;
    [SerializeField] GameObject prefab11;
    [SerializeField] GameObject prefab12;


    [SerializeField] Image image;
    [SerializeField] Slider soundsSlider;
    [SerializeField] Slider musicSlider;
    [SerializeField] AudioSource music;
    [SerializeField] AudioSource sounds;
    [SerializeField] GameObject settingsFrame;


    public void openSettings()
    {   
        musicSlider.value = PlayerPrefs.GetFloat("music", 0.0f);
        soundsSlider.value = PlayerPrefs.GetFloat("sounds", 0.0f);
        settingsFrame.active = true;
    }
    public void closeSettings()
    {
        settingsFrame.active = false;
    }
       

    public void changeSound()
    {
        sounds.volume = soundsSlider.value;
        PlayerPrefs.SetFloat("sounds",soundsSlider.value);
        PlayerPrefs.Save();
    }
    public void changeMusic()
    {
        music.volume = musicSlider.value;
        PlayerPrefs.SetFloat("music",musicSlider.value);
        PlayerPrefs.Save();
    }

    private Material[] materials;
    private GameObject[] prefabs;
    private int count = 1;

    private List<GameObject> lenta = new List<GameObject>();


    
    // Start is called before the first frame update
    void Start()
    {
        music.volume = PlayerPrefs.GetFloat("music", 0.0f);
        sounds.volume = PlayerPrefs.GetFloat("sounds", 0.0f);
        materials = new Material[] {
            material1,material2,material3,material4,material5,material6,material7
        };
        prefabs = new GameObject[] {prefab1,prefab3,prefab5,prefab7,prefab8,
            prefab9,prefab11,prefab12,
        };
        int ind1 = Random.Range(0, prefabs.Length);
        GameObject center = Instantiate(prefabs[ind1]);
        AnimationScript script = center.GetComponent<AnimationScript>();
        script.isFloating = false;
        script.isScaling = false;
        script.isAnimated = false;
        center.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
        center.transform.position = Vector3.zero;
        Item centerScript = center.AddComponent<Item>();
        centerScript.script = this;
        centerScript.ind = ind1;
        center.GetComponent<Renderer>().material = materials[Random.Range(0, materials.Length)];
        center.name = "crystal"+0+" "+0;
        CapsuleCollider2D centerCollider =  center.AddComponent<CapsuleCollider2D>();
        centerCollider.isTrigger = true;
        centerCollider.size = center.GetComponent<MeshRenderer>().bounds.size;
       
        for(int i = -3;i<=3;i++)
        {
           for(int j = -3;j<=3;j++)
            {
                if (i == 0 && j == 0) continue;
                count++;
                int ind2 = Random.Range(0, prefabs.Length);
                GameObject item = Instantiate(prefabs[ind2]);
                AnimationScript script1 = item.GetComponent<AnimationScript>();
                script1.isFloating = false;
                script1.isAnimated = false;
                script1.isScaling = false;
                item.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
                item.transform.position = new Vector3(j *0.65f,
                    i *  0.7f, 0f);
                Item itemScript = item.AddComponent<Item>();
                itemScript.script = this;
                itemScript.ind = ind2;
                item.GetComponent<Renderer>().material = materials[Random.Range(0, materials.Length)];
                item.name = "crystal" + i+" "+j;
                CapsuleCollider2D collider2D =  item.AddComponent<CapsuleCollider2D>();
                collider2D.isTrigger = true;
                collider2D.size = item.GetComponent<MeshRenderer>().bounds.size;
             
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(1);
        }
    }

    public void add(GameObject gameObject)
    {
        sounds.Play();
        count--;
        gameObject.GetComponent<Item>().canTouch = false;
        gameObject.GetComponent<AnimationScript>().isAnimated = true;
        if(lenta.Count==0)
        {
            lenta.Add(gameObject);
            gameObject.transform.position = new Vector3(
                image.transform.position.x-3.5f*0.6f
                , image.transform.position.y,0f
                );
        } else
        {
            GameObject obj = lenta.Last();
            lenta.Add(gameObject);
            gameObject.transform.position = new Vector3(
               obj.transform.position.x+0.65f
               , image.transform.position.y, 0f
               );
            for(int i = 0; i < lenta.Count; i++)
            {
                ind = lenta[i].GetComponent<Item>().ind;
                List<GameObject> filter = lenta.Where<GameObject>(
                    predicate
                ).ToList();
                if(filter.Count>=3)
                {
                    lenta.RemoveAll(predicate);
                    foreach (GameObject k in filter)
                    {
                        Destroy(k);
                    }
                    filter.Clear();
                    if(lenta.Count>0)
                    {
                        lenta[0].transform.position = new Vector3(
                                       image.transform.position.x - 3.5f * 0.6f
                                       , image.transform.position.y, 0f
                                       );
                        for(int j = 1; j < lenta.Count; j++)
                        {
                            lenta[j].transform.position = new Vector3(
                                 lenta[j-1].transform.position.x + 0.65f
                                , image.transform.position.y, 0f
                                 );
                        }
                    }
                    break;
                }
            }
            if(lenta.Count>=7)
            {
              
                SceneManager.LoadScene(4);
            }
            if(count<=0)
            {

                SceneManager.LoadScene(4);

            }
        }
    }
    private int ind;
    private bool predicate(GameObject obj)
    {   
        return obj.GetComponent<Item>().ind==ind;
    }
}
