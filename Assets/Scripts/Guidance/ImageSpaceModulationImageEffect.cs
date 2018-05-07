using UnityEngine;

namespace Guidance
{

    public class ImageSpaceModulationImageEffect : MonoBehaviour
    {	
        public Material ImageSpaceModulationMaterial;
		
        private bool _modulateImageSpace = true;
        
        private float _modulationRateMilliseconds = 100;
        private float _intensity = 1.0f;
        private float _modulationPositionX = 0.5f;
        private float _modulationPositionY = 0.5f;
	
        private float _gaussianKernelSize = 0.8f;
        private float _size = 0.2f;
		
        private Color _color1 = Color.black;
        private Color _color2 = Color.white;

        private bool _oldmodulateImageSpace;
        private float _timePassedSince;
        private int _caseIndex;

        private void Start()
        {
            _modulateImageSpace = false;
            Intensity = 0f;
        }

        void OnRenderImage (RenderTexture source, RenderTexture destination)
        {   
            if (ImageSpaceModulationMaterial)
            {
                Graphics.Blit(source,destination,ImageSpaceModulationMaterial);
            }
        }
		
        void Update ()
        {
        
            _timePassedSince += Time.deltaTime * 1000;

            if (_timePassedSince >= _modulationRateMilliseconds &&  Application.isPlaying)
            {
                _timePassedSince = 0;
                _caseIndex = (_caseIndex + 1) % 2;
                UpdateAlternatingValues();
            }
         
        }
		
        private void UpdateAlternatingValues()
        {		
		
            if (_caseIndex == 0)
            {
                ImageSpaceModulationMaterial.SetColor("_modulationValueColor", _color1);
            }
            else
            {
                ImageSpaceModulationMaterial.SetColor("_modulationValueColor", _color2);
            }
        }
		
        private void OnValidate()
        {
            Intensity = _intensity;
		
            ModulationPositionX = _modulationPositionX;

            ModulationPositionY = _modulationPositionY;
		
            Size = _size;
		
            ModulateImageSpace = _modulateImageSpace;
        }

        private void SetMaterialProperty(string property, float value)
        {
            if (ImageSpaceModulationMaterial)
            {
                ImageSpaceModulationMaterial.SetFloat(property, value);
            }
        }
        
        public int ModulationRate
        {
            set
            {
                _modulationRateMilliseconds = 1000 / (float) value;
            }
        }
	
        public float Size
        {
            set
            {
                SetMaterialProperty("_size", value);
                _size = value;
            }
            get { return _size; }
        }

        public float Intensity
        {
            set
            {
                SetMaterialProperty("_modulationIntensity", value);
                _intensity = value;
            }
            get { return _intensity; }
        }

        public Vector3 ModulationPositionXYZ
        {
            set
            {
                Vector3 viewportPoint = Camera.main.WorldToViewportPoint(value);
						
                if (viewportPoint.z > 0 && viewportPoint.x > - _size && viewportPoint.x < 1 + _size && viewportPoint.y > - _size && viewportPoint.y < 1 + _size)
                {
                    Vector3 screenPoint = Camera.main.WorldToScreenPoint(value);
								
                    _modulationPositionX = screenPoint.x / Screen.width; 
                    _modulationPositionY = screenPoint.y / Screen.height; 
			
                    SetMaterialProperty("_modulationPositionX", _modulationPositionX);
                    SetMaterialProperty("_modulationPositionY", _modulationPositionY);
                }
            }
        }

        public bool ModulateImageSpace
        {
            set
            {
                if (value != _oldmodulateImageSpace)
                {
                    _oldmodulateImageSpace = value;
                    enabled = value;
                    _modulateImageSpace = value;
                }
			
            }
            get { return _modulateImageSpace; }
        }

	
        public float ModulationPositionY
        {
            set
            {
                _modulationPositionY = value; 
			
                SetMaterialProperty("_modulationPositionY", value);
            }
        }
	
        public float ModulationPositionX
        {
            set
            {
                _modulationPositionX = value; 
			
                SetMaterialProperty("_modulationPositionX", value);
            }
        }
		
    }
}