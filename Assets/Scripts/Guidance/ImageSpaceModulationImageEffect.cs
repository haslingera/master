using UnityEngine;

namespace Guidance
{
    [ExecuteInEditMode]
    public class ImageSpaceModulationImageEffect : MonoBehaviour
    {	
        public Material ImageSpaceModulationMaterial;
		
        [Header("General")]
        public bool _modulateImageSpace = true;

        [Tooltip("Modulation Rate in HZ")]
        public int _modulationRate = 10;
        public bool AlternateModulation;
	
        [Space(10)]
        [Header("Modulation Blend Mode")]
        public ModulationBlendModeEnum _blendMode = ModulationBlendModeEnum.Bailey;
        public bool _optimizeForPerceivedBrightness;
        [Range(0, 1)]
        public float _intensity = 1.0f;

        [Space(10)]
        [Header("Modulation Position")]
        [Range(0, 1)]
        public float _modulationPositionX = 0.5f;
        [Range(0, 1)]
        public float _modulationPositionY = 0.5f;
	
        [Space(10)] 
        [Header("Modulation Size")]
        public FalloffTypeModeEnum _falloffType = FalloffTypeModeEnum.Gaussian;
        [Range(0, 1)]
        public float _gaussianKernelSize = 0.8f;
        [Range(0, 1)]
        public float _size = 0.2f;
	
        [Space(10)]
        [Header("Modulation Type")]
        public ModulationTypeModeEnum _modulationType = ModulationTypeModeEnum.Color;
	
        public Color _color1 = Color.red;
        public Color _color2 = Color.blue;
        public float _brightness1;
        public float _brightness2;
        public float _contrast1 = 1f; 
        public float _contrast2 = 1f;
        public float _saturation1;
        public float _saturation2;
        public Texture2D _noise;
        public float _noiseScale = 1;
			
        public enum ModulationBlendModeEnum { Bailey, Multiply, Screen, Overlay, Darken, Lighten }
        public enum ModulationTypeModeEnum { None, Color, Brightness, Contrast, Saturation, InverseColor, InverseBrightness, InverseBlackAndWhite, Noise }
        public enum FalloffTypeModeEnum { None, Linear, Gaussian }
		
        private Material _ismMaterial;
        private ImageSpaceModulationImageEffect _ism;

        private ModulationTypeModeEnum _oldModulationType;
        private bool _oldmodulateImageSpace;
        private int _oldModulationRate;
        private float _currentModulationRateMilliseconds;
        private bool _testRunning;
	
        private float ModulationRateMilliseconds
        {
            get
            {
                if (_modulationRate == _oldModulationRate) return _currentModulationRateMilliseconds;
                _oldModulationRate = _modulationRate;
                _currentModulationRateMilliseconds = 1000 / (float) _modulationRate;
                return _currentModulationRateMilliseconds;
            }
        }
        private float _timePassedSince;
        private int _caseIndex;

        void OnRenderImage (RenderTexture source, RenderTexture destination)
        {
            if (ImageSpaceModulationMaterial)
            {
                Graphics.Blit(source,destination,ImageSpaceModulationMaterial);
            }
        }
		
        void Update ()
        {
            if (AlternateModulation)
            {
                _timePassedSince += Time.deltaTime * 1000;

                if (_timePassedSince >= ModulationRateMilliseconds &&  Application.isPlaying)
                {
                    _timePassedSince = 0;
                    _caseIndex = (_caseIndex + 1) % 2;
                    UpdateAlternatingValues();
                }
            }
            else
            {
                _timePassedSince = 0;
            }
        }
		
        private void UpdateAlternatingValues()
        {		
		
            switch (_modulationType)
            {
                case ModulationTypeModeEnum.Color:
                    if (_caseIndex == 0)
                    {
                        ImageSpaceModulationMaterial.SetColor("_modulationValueColor", _color1);
                    }
                    else
                    {
                        ImageSpaceModulationMaterial.SetColor("_modulationValueColor", _color2);
                    }
                    break;
                case ModulationTypeModeEnum.Brightness:
                    if (_caseIndex == 0)
                    {
                        ImageSpaceModulationMaterial.SetFloat("_modulationValueBrightness", _brightness1);
                    }
                    else
                    {
                        ImageSpaceModulationMaterial.SetFloat("_modulationValueBrightness", _brightness2);
                    }
                    break;
                case ModulationTypeModeEnum.Contrast:
                    if (_caseIndex == 0)
                    {
                        ImageSpaceModulationMaterial.SetFloat("_modulationContrast", _contrast1);
                    }
                    else
                    {
                        ImageSpaceModulationMaterial.SetFloat("_modulationContrast", _contrast2);
                    }
                    break;
                case ModulationTypeModeEnum.Saturation:
                    if (_caseIndex == 0)
                    {
                        ImageSpaceModulationMaterial.SetFloat("_modulationSaturation", _saturation1);
                    }
                    else
                    {
                        ImageSpaceModulationMaterial.SetFloat("_modulationSaturation", _saturation2);
                    }
                    break;
				
                case ModulationTypeModeEnum.InverseColor:
                case ModulationTypeModeEnum.InverseBrightness:
                case ModulationTypeModeEnum.InverseBlackAndWhite:
                case ModulationTypeModeEnum.Noise:
                    if (_caseIndex == 0)
                    {
                        ImageSpaceModulationMaterial.SetInt("_modulationType", (int)_modulationType);
                    }
                    else
                    {
                        ImageSpaceModulationMaterial.SetInt("_modulationType", (int)ModulationTypeModeEnum.None);
                    }
                    break;
            }
        }
		
        private void OnValidate()
        {
		
            BlendMode = _blendMode;
		
            OptimizeForPerceivedBrightness = _optimizeForPerceivedBrightness;
		
            Intensity = _intensity;
		
            ModulationPositionX = _modulationPositionX;

            ModulationPositionY = _modulationPositionY;
		
            FalloffType = _falloffType;
		
            GaussianKernelSize = _gaussianKernelSize;
		
            Size = _size;
		
            ModulationType = _modulationType;
		
            ModulateImageSpace = _modulateImageSpace;

            FalloffType = _falloffType;

            GaussianKernelSize = _gaussianKernelSize;

            Color1 = _color1;

            Brightness1 = _brightness1;

            Contrast1 = _contrast1;

            Saturation1 = _saturation1;

            Noise = _noise;

            NoiseScale = _noiseScale;

        }

        private void SetMaterialProperty(string property, float value)
        {
            if (ImageSpaceModulationMaterial)
            {
                ImageSpaceModulationMaterial.SetFloat(property, value);
            }
        }
        
        private void SetMaterialProperty(string property, int value)
        {
            if (ImageSpaceModulationMaterial)
            {
                ImageSpaceModulationMaterial.SetInt(property, value);
            }
        }
        
        private void SetMaterialProperty(string property, Color value)
        {
            if (ImageSpaceModulationMaterial)
            {
                ImageSpaceModulationMaterial.SetColor(property, value);
            }
        }
        
        private void SetMaterialProperty(string property, Texture2D value)
        {
            if (ImageSpaceModulationMaterial)
            {
                ImageSpaceModulationMaterial.SetTexture(property, value);
            }
        }
		
        //------------------------------------------------------------------------------
        //-------------------------------GETTERS&SETTERS--------------------------------
        //------------------------------------------------------------------------------
	
        public ModulationTypeModeEnum ModulationType
        {
            set
            {
                SetMaterialProperty("_modulationType", (int)value);
                _modulationType = value;
            }
            get { return _modulationType; }
		
        }
	
        public float GaussianKernelSize
        {
            set
            {
                SetMaterialProperty("_kernelSize", value);
                _gaussianKernelSize = value;
            }
            get { return _gaussianKernelSize; }
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
	
        public FalloffTypeModeEnum FalloffType
        {
            set
            {
                SetMaterialProperty("_falloffType", (int)value);
                _falloffType = value;
            }
            get { return _falloffType; }
		
        }

        public bool OptimizeForPerceivedBrightness
        {
            set
            {
                SetMaterialProperty("_automaticModulationIntensity", value ? 1 : 0);
                _optimizeForPerceivedBrightness = value;
            }
            get { return _optimizeForPerceivedBrightness; }
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

        public ModulationBlendModeEnum BlendMode
        {
            set
            {
                if (ImageSpaceModulationMaterial)
                {
                    SetMaterialProperty("_modulationBlendMode", (int)value);
                    _blendMode = value;
                }
            }

            get { return _blendMode; }
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

        public Color Color1
        {
            set
            {
                SetMaterialProperty("_modulationValueColor", value);
                _color1 = value;
            }

            get { return _color1; }
        }
	
        public float Brightness1
        {
            set
            {
                SetMaterialProperty("_modulationValueBrightness", value);
                _brightness1 = value;
            }

            get { return _brightness1; }
        }
	
        public float Contrast1
        {
            set
            {
                SetMaterialProperty("_modulationContrast", value);
                _contrast1 = value;
            }

            get { return _contrast1; }
        }
	
        public float Saturation1
        {
            set
            {
                SetMaterialProperty("_modulationSaturation", value);
                _saturation1 = value;
            }

            get { return _saturation1; }
        }
	
        public Color Color2
        {
            set
            {
                _color2 = value;
            }

            get { return _color2; }
        }
	
        public float Brightness2
        {
            set
            {
                _brightness2 = value;
            }

            get { return _brightness2; }
        }
	
        public float Contrast2
        {
            set
            {
                _contrast2 = value;
            }

            get { return _contrast2; }
        }
	
        public float Saturation2
        {
            set
            {
                SetMaterialProperty("_modulationSaturation", value);
                _saturation1 = value;
            }

            get { return _saturation2; }
        }

        public Texture2D Noise
        {
            set
            {
                SetMaterialProperty("_noise", value);
                _noise = value;
            }

            get { return _noise; }
        }
	
        public float NoiseScale
        {
            set
            {
                if (value < 0) { value = 0; }
                _noiseScale = value;
                SetMaterialProperty("_noiseScale", value);			
            }

            get { return _noiseScale; }
        }
		
    }
}