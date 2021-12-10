using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Board : MonoBehaviour {
    [SerializeField] private Slider _rowsSlider;
    [SerializeField] private Slider _thicknessSlider;
    [SerializeField, HideInInspector] private Slider _offset;
    [SerializeField, HideInInspector] private Toggle _enable3D;

    [SerializeField] private GameObject _planePrefab;
    [SerializeField] private GameObject _borderPrefab;
    [SerializeField, HideInInspector] private GameObject _hitBoxPrefab;

    private Dictionary<HitBox, string> _fields = new Dictionary<HitBox, string>();

    void Awake() {
        GameManager.Instance.SetBoard(this);
        _rowsSlider.onValueChanged.AddListener(OnSliderValueChanged);
        _thicknessSlider.onValueChanged.AddListener(OnSliderValueChanged);
        _offset.onValueChanged.AddListener(OnSliderValueChanged);
        _enable3D.onValueChanged.AddListener(b => {
            _offset.gameObject.SetActive(b);
            OnSliderValueChanged();
        });

        _offset.gameObject.SetActive(_enable3D.isOn);

        Generate((int) _rowsSlider.value, _thicknessSlider.value, _offset.value);
    }

    void OnSliderValueChanged(float value = 0) {
        transform.Clear();
        Generate((int) _rowsSlider.value, _thicknessSlider.value, _offset.value);
    }

    public void Generate(int rows, float thickness, float offset) {
        transform.Clear();
        GameManager.Instance.Set(rows, _enable3D.isOn);
        GameManager.Instance.Clear();

        for (int i = 0; i < (_enable3D.isOn ? rows : 1); i++) {
            var parent = Instantiate(_planePrefab, transform);
            var parentPosition = parent.transform.position;
            var centeredIndex = _enable3D.isOn ? (rows - 1f) / 2f * -1f + i : 0f;
            parentPosition.y += centeredIndex * offset;
            parent.transform.position = parentPosition;

            GenerateField(rows, thickness, parent, i);
        }
    }

    private void GenerateField(int rows, float thickness, GameObject parent, int row) {
        var size = Vector3.Scale(parent.GetComponent<MeshFilter>().mesh.bounds.size,
            parent.transform.localScale);

        //generate borders
        for (int i = 1; i < rows; i++) {
            var totalSize = size.x - thickness * (rows - 1);
            var offset = totalSize / rows;
            var position = offset * i + thickness * i - thickness / 2.0f - size.x / 2.0f;
            //var start = (size.x / 2.0f) * -1.0f;
            //var offset = size.x / rows * i;
            //var position = start + offset;

            var borderX = Instantiate(_borderPrefab, parent.transform);
            borderX.transform.localScale = new Vector3(thickness, thickness, size.x);
            borderX.transform.localPosition = new Vector3(position, thickness / 2, 0);

            var borderY = Instantiate(_borderPrefab, parent.transform);
            borderY.transform.localScale = new Vector3(size.x, thickness, thickness);
            borderY.transform.localPosition = new Vector3(0, thickness / 2, position);
        }

        // generate hitboxes
        for (int i = 0; i < rows; i++) {
            for (int j = 0; j < rows; j++) {
                var totalSize = size.x - thickness * (rows - 1);
                var offset = totalSize / rows;
                var positionX = thickness * i + offset * i + offset / 2.0f - size.x / 2.0f;
                var positionY = thickness * j + offset * j + offset / 2.0f - size.x / 2.0f;
                //var totalSize = size.x - thickness * (rows - 1);
                //var offset = totalSize / rows;
                //var positionX = thickness * i + offset * i + offset / 2.0f - size.x / 2.0f;
                //var positionY = thickness * j + offset * j + offset / 2.0f - size.x / 2.0f;

                var hitbox = Instantiate(_hitBoxPrefab, parent.transform);
                var localScale = new Vector3(offset,
                    thickness,
                    offset);
                hitbox.transform.localScale = localScale;
                hitbox.transform.localPosition = new Vector3(positionX,
                    localScale.y / 2,
                    positionY);

                GameManager.Instance.AddHitBox(hitbox.GetComponent<HitBox>(), i, j, row);
            }
        }
    }

    public void Reset() {
        OnSliderValueChanged();
    }
}