using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDestroy : MonoBehaviour
{
   public static DontDestroy instance;
   public static bool _switched;
   private void Awake()
   {
      DontDestroyOnLoad(gameObject);
   }
   private void OnEnable()
   {
      if(instance  == null)
         instance = this;
      else
      {
         Destroy(gameObject);
         return;
      }
      
      SceneManager.activeSceneChanged += SceneChanged;
   }

   private void OnDisable() => SceneManager.activeSceneChanged -= SceneChanged;
   private void SceneChanged(Scene oldScene, Scene newScene) {
      if (newScene.buildIndex - oldScene.buildIndex == 1) Destroy(gameObject);
   }
}
