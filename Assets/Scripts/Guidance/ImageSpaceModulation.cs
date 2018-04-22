using System.CodeDom;
using Guidance;
using UnityEngine;

[ExecuteInEditMode]
public class ImageSpaceModulation : MonoBehaviour {

	[Header("General")]
	[SerializeField] private bool _modulateImageSpace = true;

	[Tooltip("Modulation Rate in HZ")]
	[SerializeField] private int _modulationRate = 10;
	public bool AlternateModulation;
	public Material _debugMaterial;
	
	[Space(10)]
	[Header("Modulation Blend Mode")]
	[SerializeField] private ModulationBlendModeEnum _blendMode = ModulationBlendModeEnum.Bailey;
	[SerializeField] private bool _optimizeForPerceivedBrightness;
	[Range(0, 1)]
	[SerializeField] private float _intensity = 1.0f;

	[Space(10)]
	[Header("Modulation Position")]
	[Range(0, 1)]
	[SerializeField] private float _modulationPositionX = 0.5f;
	[Range(0, 1)]
	[SerializeField] private float _modulationPositionY = 0.5f;
	
	[Space(10)] 
	[Header("Modulation Size")]
	[SerializeField] private FalloffTypeModeEnum _falloffType = FalloffTypeModeEnum.Gaussian;
	[Range(0, 1)]
	[SerializeField] private float _gaussianKernelSize = 0.8f;
	[Range(0, 1)]
	[SerializeField] private float _size = 0.2f;
	
	[Space(10)]
	[Header("Modulation Type")]
	[SerializeField] private ModulationTypeModeEnum _modulationType = ModulationTypeModeEnum.Color;
	
	[HideInInspector] [SerializeField] private Color _color1 = Color.red;
	[HideInInspector] [SerializeField] private Color _color2 = Color.blue;
	[HideInInspector] [SerializeField] private float _brightness1;
	[HideInInspector] [SerializeField] private float _brightness2;
	[HideInInspector] [SerializeField] private float _contrast1 = 1f; 
	[HideInInspector] [SerializeField] private float _contrast2 = 1f;
	[HideInInspector] [SerializeField] private float _saturation1;
	[HideInInspector] [SerializeField] private float _saturation2;
	[HideInInspector] [SerializeField] private Texture2D _noise;
	[HideInInspector] [SerializeField] private float _noiseScale = 1;
			
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

	private void OnEnable()
	{
		
		if (Camera.main.gameObject.GetComponent<ImageSpaceModulationImageEffect>() == null)
		{
			_ism = Camera.main.gameObject.AddComponent<ImageSpaceModulationImageEffect>();
			_ismMaterial = new Material(Shader.Find("ImageSpaceModulation/Standard"));
			_ism.ImageSpaceModulationMaterial = _ismMaterial;
		}
		else
		{
			_ism = Camera.main.gameObject.GetComponent<ImageSpaceModulationImageEffect>();
			_ismMaterial = _ism.ImageSpaceModulationMaterial;
		}
	}

	private void Start()
	{
		_oldmodulateImageSpace = _modulateImageSpace;
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
					_ismMaterial.SetColor("_modulationValueColor", _color1);
				}
				else
				{
					_ismMaterial.SetColor("_modulationValueColor", _color2);
				}
				break;
			case ModulationTypeModeEnum.Brightness:
				if (_caseIndex == 0)
				{
					_ismMaterial.SetFloat("_modulationValueBrightness", _brightness1);
				}
				else
				{
					_ismMaterial.SetFloat("_modulationValueBrightness", _brightness2);
				}
				break;
			case ModulationTypeModeEnum.Contrast:
				if (_caseIndex == 0)
				{
					_ismMaterial.SetFloat("_modulationContrast", _contrast1);
				}
				else
				{
					_ismMaterial.SetFloat("_modulationContrast", _contrast2);
				}
				break;
			case ModulationTypeModeEnum.Saturation:
				if (_caseIndex == 0)
				{
					_ismMaterial.SetFloat("_modulationSaturation", _saturation1);
				}
				else
				{
					_ismMaterial.SetFloat("_modulationSaturation", _saturation2);
				}
				break;
				
			case ModulationTypeModeEnum.InverseColor:
			case ModulationTypeModeEnum.InverseBrightness:
			case ModulationTypeModeEnum.InverseBlackAndWhite:
			case ModulationTypeModeEnum.Noise:
				if (_caseIndex == 0)
				{
					_ismMaterial.SetInt("_modulationType", (int)_modulationType);
				}
				else
				{
					_ismMaterial.SetInt("_modulationType", (int)ModulationTypeModeEnum.None);
				}
				break;
		}
	}
	
	private void OnValidate()
	{
		
		if (_ism == null)
		{
			OnEnable();
		}

		DebugMaterial = _debugMaterial;
				
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

	}
	
	//------------------------------------------------------------------------------
	//-------------------------------GETTERS&SETTERS--------------------------------
	//------------------------------------------------------------------------------
	
	public ModulationTypeModeEnum ModulationType
	{
		set
		{
			_ismMaterial.SetInt("_modulationType", (int)value);
			_modulationType = value;
		}
		get { return _modulationType; }
		
	}
	
	public float GaussianKernelSize
	{
		set
		{
			_ismMaterial.SetFloat("_kernelSize", value);
			_gaussianKernelSize = value;
		}
		get { return _gaussianKernelSize; }
	}
	
	public float Size
	{
		set
		{
			_ismMaterial.SetFloat("_size", value);
			_size = value;
		}
		get { return _size; }
	}
	
	public FalloffTypeModeEnum FalloffType
	{
		set
		{
			_ismMaterial.SetInt("_falloffType", (int)value);
			_falloffType = value;
		}
		get { return _falloffType; }
		
	}

	public bool OptimizeForPerceivedBrightness
	{
		set
		{
			_ismMaterial.SetInt("_automaticModulationIntensity", value ? 1 : 0);
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
				_ism.enabled = value;
				_modulateImageSpace = value;
			}
			
		}
		get { return _modulateImageSpace; }
	}

	public ModulationBlendModeEnum BlendMode
	{
		set
		{
			_ismMaterial.SetInt("_modulationBlendMode", (int)value);
			_blendMode = value;
			
		}

		get { return _blendMode; }
	}

	public float Intensity
	{
		set
		{
			_ismMaterial.SetFloat("_modulationIntensity", value);
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
			
				_ismMaterial.SetFloat("_modulationPositionX", _modulationPositionX);
				_ismMaterial.SetFloat("_modulationPositionY", _modulationPositionY);
			}
			
		}
	}
	
	public float ModulationPositionY
	{
		set
		{
			_modulationPositionY = value; 
			
			_ismMaterial.SetFloat("_modulationPositionY", value);
		}
	}
	
	public float ModulationPositionX
	{
		set
		{
			_modulationPositionX = value; 
			
			_ismMaterial.SetFloat("_modulationPositionX", value);
		}
	}

	public Color Color1
	{
		set
		{
			_ismMaterial.SetColor("_modulationValueColor", value);
			_color1 = value;
		}

		get { return _color1; }
	}
	
	public float Brightness1
	{
		set
		{
			_ismMaterial.SetFloat("_modulationValueBrightness", value);
			_brightness1 = value;
		}

		get { return _brightness1; }
	}
	
	public float Contrast1
	{
		set
		{
			_ismMaterial.SetFloat("_modulationContrast", value);
			_contrast1 = value;
		}

		get { return _contrast1; }
	}
	
	public float Saturation1
	{
		set
		{
			_ismMaterial.SetFloat("_modulationSaturation", value);
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
			_ismMaterial.SetFloat("_modulationSaturation", value);
			_saturation1 = value;
		}

		get { return _saturation2; }
	}

	public Texture2D Noise
	{
		set
		{
			_ismMaterial.SetTexture("_noise", value);
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
			_ismMaterial.SetFloat("_noiseScale", value);			
		}

		get { return _noiseScale; }
	}
	
	 public Material DebugMaterial
	{
		set
		{
			if (value != null)
			{
				_ism.ImageSpaceModulationMaterial = value;
			}
			else
			{

				if (_ismMaterial == null)
				{
					_ismMaterial = new Material(Shader.Find("ImageSpaceModulation/Standard"));
				}
				
				_ism.ImageSpaceModulationMaterial = _ismMaterial;
				
			}
			
			_debugMaterial = value;
			
		}
	}
		
}
