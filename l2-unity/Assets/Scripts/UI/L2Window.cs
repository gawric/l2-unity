using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class L2Window : MonoBehaviour
{
    protected VisualElement _root;
    protected VisualTreeAsset _windowTemplate;
    protected VisualElement _windowEle;
    protected bool _isWindowHidden = false;
    protected MouseOverDetectionManipulator _mouseOverDetection;

    void Start()
    {
        _isWindowHidden = false;
        LoadAssets();
    }

    protected abstract void LoadAssets();

    protected VisualTreeAsset LoadAsset(string assetPath)
    {
        VisualTreeAsset asset = Resources.Load<VisualTreeAsset>(assetPath);
        if (asset == null)
        {
            Debug.LogError($"Could not load {assetPath} template.");
        }

        return asset;
    }

    public void AddWindow(VisualElement root)
    {
        if (_windowTemplate == null)
        {
            return;
        }
        StartCoroutine(BuildWindow(root));
    }

    protected virtual void InitWindow(VisualElement root)
    {
        _root = root;
        _windowEle = _windowTemplate.Instantiate()[0];
        _mouseOverDetection = new MouseOverDetectionManipulator(_windowEle);
        _windowEle.AddManipulator(_mouseOverDetection);

        if (_isWindowHidden)
        {
            _mouseOverDetection.Disable();
        }

        root.Add(_windowEle);
    }

    protected abstract IEnumerator BuildWindow(VisualElement root);

    public virtual void HideWindow()
    {
        _isWindowHidden = true;
        _windowEle.style.display = DisplayStyle.None;
        _mouseOverDetection.Disable();
    }

    public virtual void ShowWindow()
    {
        _isWindowHidden = false;
        _windowEle.style.display = DisplayStyle.Flex;
        _mouseOverDetection.Enable();
    }

    public virtual void ToggleHideWindow()
    {
        if (_isWindowHidden)
        {
            ShowWindow();
        }
        else
        {
            HideWindow();
        }
    }

    protected Label GetLabelById(string id)
    {
        VisualElement e = GetElementById(id);
        if (e == null)
        {
            return null;
        }

        return (Label)e;
    }

    protected VisualElement GetElementById(string id)
    {
        var btn = _windowEle.Q<VisualElement>(id);
        if (btn == null)
        {
            Debug.LogError(id + " can't be found.");
            return null;
        }

        return btn;
    }

    public VisualElement GetElementByClass(string className)
    {
        var btn = _windowEle.Q<VisualElement>(null, className);
        if (btn == null)
        {
            Debug.LogError(className + " can't be found.");
            return null;
        }

        return btn;
    }


    public virtual void BringToFront()
    {
    }

    public virtual void SendToBack()
    {
    }

    public bool MouseOverThisWindow()
    {
        return _mouseOverDetection.MouseOver;
    }
}
