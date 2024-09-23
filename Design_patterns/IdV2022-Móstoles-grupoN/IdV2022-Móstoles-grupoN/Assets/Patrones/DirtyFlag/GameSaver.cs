using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Patrones.DirtyFlag.Interfaces;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Patrones.DirtyFlag
{
    /*
     * Este codigo esta basado en el ejercicio de clase del DirtyFlag, ya que la funcion de guardado no tiene mucha variacion.
     * Se ha añadido la funcion de reset que vacia el fichero. 
     */
    public class GameSaver : MonoBehaviour
    {
        
        [SerializeField]
        public bool RestoreOnLoad = true;

        [SerializeField]
        public bool Reset = false;

        private void Awake()
        {   
            guardaFichero = Application.dataPath + "/savedata.save";
            objects =  new Dictionary<string, ISaveableGameObjectData>();

            if (Reset)
            {
                ResetGame();
            }

            if (RestoreOnLoad)
            {
                LoadGame();    
            }

            StartCoroutine(SaveGame());
        }
       

        
        private bool aplicaDirty = false;
        
        public void SetSceneDirty()
        {
            aplicaDirty = true;
        }
        

        
        private string guardaFichero;
        private Dictionary<string, ISaveableGameObjectData> objects;

        private IEnumerator SaveGame()
        {
            while (true)
            {
                if (aplicaDirty)
                {
                    Scene scene = gameObject.scene;

                    GameObject[] sceneObjects = scene.GetRootGameObjects();
                    foreach (GameObject sceneObject in sceneObjects)
                    {
                        ISaveableGameObject obj = sceneObject.GetComponent<ISaveableGameObject>();
                        if (obj != null && obj.NeedsSaving())
                        {
                            Debug.Log($"Saving {sceneObject.name}: {JsonUtility.ToJson(obj.GetSavedObject())}");
                            objects[sceneObject.name] = obj.GetSavedObject();
                            yield return null;
                        }
                    }

                    BinaryFormatter formatter = new BinaryFormatter();
                    FileStream fileStream = File.Create(guardaFichero);
                    formatter.Serialize(fileStream, objects);
                    fileStream.Flush();
                    fileStream.Close();
                    aplicaDirty = false;
                }
               
                
                yield return new WaitForSecondsRealtime(10);
            }
        }

        public void LoadGame()
        {
            if (File.Exists(guardaFichero))
            {
                Debug.Log($"{guardaFichero} found, starting restore.");
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream fileStream = File.OpenRead(guardaFichero);
                objects = (Dictionary<string, ISaveableGameObjectData>)formatter.Deserialize(fileStream);
                fileStream.Close();

                foreach (var objectData in objects)
                {
                    GameObject gameObject = GameObject.Find(objectData.Key);
                    if (gameObject != null)
                    {
                        ISaveableGameObject saveableGameObject = gameObject.GetComponent<ISaveableGameObject>();
                        if (saveableGameObject != null)
                        {
                            Debug.Log($"Restoring {objectData.Key}: {JsonUtility.ToJson(saveableGameObject)}");
                            saveableGameObject.ToGameObject(objectData.Value);    
                        }
                      
                            
                    }
                }
            }
         
        }

        public void ResetGame()
        {
            if (File.Exists(guardaFichero))
            {
                File.Delete(guardaFichero); 
            }   
        }
    }
}