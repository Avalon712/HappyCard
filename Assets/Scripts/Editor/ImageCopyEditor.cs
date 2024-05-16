using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace HappyCard.Editor
{
    public class ImageCopyEditor : EditorWindow
    {
        public List<GameObject> _objects;
        private SerializedProperty _serializedObjs;
        private SerializedObject _window;
        private Vector2 _scrollPos = Vector2.zero;
        private string _path = "Assets/";

        [MenuItem("HappyCard/ImageCopyEditor")]
        public static void OpenEditorWindow()
        {
            var window = GetWindow<ImageCopyEditor>("ͼƬ��Դ����༭��");
            window.position = new Rect(320, 240, 700, 160);
            window.Show();

            window._window = new SerializedObject(window);
            window._serializedObjs = window._window.FindProperty("_objects");
        }


        private void OnGUI()
        {
            _window.Update(); //Update������EditorGUILayout.Xxx֮ǰ

            _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("�������ͼƬ��ŵ�Ŀ¼");
            _path = EditorGUILayout.TextField(_path);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Ҫ�ռ�������ͼƬ");
            EditorGUILayout.PropertyField(_serializedObjs);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();

            if (GUILayout.Button("�ռ�����ͼƬȻ�󿽱�,�����ÿ�����ľ���ͼ"))
            {
                foreach (var obj in _objects)
                {
                    CopyImages(obj);
                }
            }
            EditorGUILayout.Space();

            EditorGUILayout.EndScrollView();

            _window.ApplyModifiedProperties();
        }

        private void CopyImages(GameObject obj)
        {
            if (string.IsNullOrEmpty(_path)) { Debug.LogWarning("���Ŀ¼����Ϊ��"); return; }
            if (!_path.EndsWith('/')) { _path = _path + '/'; }

            Dictionary<Image, string> imgs = new();

            try
            {
                //������
                AssetDatabase.StartAssetEditing();

                using (var it = GetAllGameObject(obj).GetEnumerator())
                {
                    while (it.MoveNext())
                    {
                        Image image = it.Current.GetComponent<Image>();
                        if (image != null)
                        {
                            Sprite sprite = image.sprite;
                            if (sprite != null)
                            {
                                string spritePath = AssetDatabase.GetAssetPath(sprite);

                                //�ų�Unity�Դ�����Դ
                                if (!spritePath.StartsWith("Assets")) { continue; }

                                string[] fileNames = spritePath.Split('/');
                                string outputPath = _path + fileNames[fileNames.Length - 1];

                                //Debug.Log("���·����"+outputPath);
                                //Debug.Log(abPath);

                                if (!File.Exists(outputPath)) 
                                {
                                    //����
                                    if (!AssetDatabase.CopyAsset(spritePath, outputPath))
                                    {
                                        Debug.LogWarning($"�ʲ���{spritePath}������{outputPath}ʧ��");
                                    }
                                }

                                imgs.Add(image, outputPath);
                            }
                        }
                    }
                }
            }
            finally
            {
                AssetDatabase.StopAssetEditing();
            }
            

            AssetDatabase.Refresh();

            foreach (var img in imgs.Keys)
            {
                img.sprite = AssetDatabase.LoadAssetAtPath<Sprite>(imgs[img]);
            }
        }

        private IEnumerable<GameObject> GetAllGameObject(GameObject root)
        {
            yield return root;

            Queue<Transform> queue = new Queue<Transform>();
            queue.Enqueue(root.transform);

            while (queue.Count > 0)
            {
                Transform transform = queue.Dequeue();

                for (int i = 0; i < transform.childCount; i++)
                {
                    Transform child = transform.GetChild(i);

                    yield return child.gameObject;

                    if (child.childCount != 0) //��Ҷ�ӽڵ������
                    {
                        queue.Enqueue(child);
                    }
                }
            }

        }
    }
}

