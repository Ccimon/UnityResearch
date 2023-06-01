using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace AfterEffect
{
    public class GhostMesh<T> where T : UnityEngine.Object
    {
        private T self;

        public void SaveRender(T obj)
        {
            self = Object.Instantiate(obj);
        }
    }
    public class GhostEffect: MonoBehaviour
    {
        public int GhostCount = 0;

        private Queue<GameObject> _ghostBuffer ;
        private GameObject _meta;
        private Image _image;
        private GhostMesh<Sprite> _item;
        private GameObject _ghost;
        private void Start()
        {
            _ghostBuffer = new Queue<GameObject>(GhostCount);
            _image = GetComponent<Image>();
            _item = new GhostMesh<Sprite>();
            _item.SaveRender(_image.sprite);
            
            _meta = Instantiate(gameObject,transform.parent);
            _meta.name = "Meta";
            _meta.transform.parent = null;
            Destroy(_meta.GetComponent<GhostEffect>());
        }
        
        void LateUpdate()
        {
            _item.SaveRender(_image.sprite);
            var obj = CloneSelf();
            obj.transform.position = transform.position;
        }

        private void OnRenderImage(RenderTexture src, RenderTexture dest)
        {
            
        }

        private GameObject CloneSelf()
        {
            if (_ghostBuffer.Count >= GhostCount)
            {
                var clone = _ghostBuffer.Dequeue();
                Destroy(clone);
            }
            
            var obj = Instantiate(_meta,transform.parent);
            obj.name = "Clone" + Random.Range(0f, 1f);
            _ghostBuffer.Enqueue(obj);
            return obj;
        }
    }
}

