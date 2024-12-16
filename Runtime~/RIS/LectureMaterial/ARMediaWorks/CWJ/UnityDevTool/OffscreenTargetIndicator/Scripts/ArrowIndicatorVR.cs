using UnityEngine;
using System.Collections;

namespace CWJ
{
	public class ArrowIndicatorVR : ArrowIndicatorAbstract
	{
		public Vector3 VR_scale;
		public SpriteRenderer arrowImg;
		public TextMesh distanceText;

		public override bool onScreen
		{
			get
			{
				return _onScreen;
			}

			set
			{
				if (_onScreenNextValue != value)
				{
					_onScreenNextValue = value;
					if (value)
					{
						if (indicator.transition == IndicatorSetting.Transition.None)
						{
							_onScreen = value;
							if (indicator.showOnScreen)
							{
								gameObject.SetActive(true);
								arrowImg.sprite = indicator.onScreenSprite;
								arrowImg.color = indicator.onScreenColor;
							}
							else
							{
								gameObject.SetActive(false);
							}
						}
						else
						{
							fadingToOn = true;
							fadingToOff = false;
						}
					}
					else
					{
						if (indicator.transition == IndicatorSetting.Transition.None)
						{
							_onScreen = value;
							if (indicator.showOffScreen)
							{
								gameObject.SetActive(true);
								arrowImg.sprite = indicator.offScreenSprite;
								arrowImg.color = indicator.offScreenColor;
							}
							else
							{
								gameObject.SetActive(false);
							}
						}
						else
						{
							fadingToOn = false;
							fadingToOff = true;
						}
					}
					timeStartLerp = Time.time;
					fadingUp = false;
				}
			}
		}

		public override void UpdateEffects()
		{
			if (fadingToOn || fadingToOff)
			{
				elapsedTime = Time.time - timeStartLerp;
				if ((fadingToOn && !indicator.showOffScreen) || (fadingToOff && !indicator.showOnScreen))
				{
					elapsedTime += indicator.transitionDuration;
				}
				if (elapsedTime < indicator.transitionDuration)
				{
					FadingDownValues();
				}
				else if (elapsedTime < indicator.transitionDuration * 2)
				{
					if (!fadingUp)
					{
						arrowImg.sprite = fadingToOff ? indicator.offScreenSprite : indicator.onScreenSprite;
						arrowImg.color = fadingToOff ? indicator.offScreenColor : indicator.onScreenColor;
						_onScreen = _onScreenNextValue;
						fadingUp = true;
					}
					if ((onScreen && !indicator.showOnScreen) || (!onScreen && !indicator.showOffScreen))
					{
						gameObject.SetActive(false);
						fadingToOn = false;
						fadingToOff = false;
					}
					else
					{
						gameObject.SetActive(true);
						FadingUpValues();
					}
				}
				else
				{
					if (!fadingUp)
					{
						arrowImg.sprite = fadingToOff ? indicator.offScreenSprite : indicator.onScreenSprite;
						arrowImg.color = fadingToOff ? indicator.offScreenColor : indicator.onScreenColor;
						_onScreen = _onScreenNextValue;
						fadingUp = true;
					}
					EndFadingValues();
					fadingToOn = false;
					fadingToOff = false;
				}
			}
		}

		private void FadingDownValues()
		{
			if (indicator.transition == IndicatorSetting.Transition.Fading)
			{
				if (onScreen)
				{
					transColor = indicator.onScreenColor;
				}
				else
				{
					transColor = indicator.offScreenColor;
				}
				arrowImg.color = Color32.Lerp(transColor, new Color32(System.Convert.ToByte(transColor.r * 255),
																		System.Convert.ToByte(transColor.g * 255),
																		System.Convert.ToByte(transColor.b * 255), 0),
																		elapsedTime / indicator.transitionDuration);
			}
			if (indicator.transition == IndicatorSetting.Transition.Scaling)
			{
				transform.localScale = Vector3.Lerp(VR_scale, Vector3.zero, elapsedTime / indicator.transitionDuration);
			}
		}

		private void FadingUpValues()
		{
			if (indicator.transition == IndicatorSetting.Transition.Fading)
			{
				if (onScreen)
				{
					transColor = indicator.onScreenColor;
				}
				else
				{
					transColor = indicator.offScreenColor;
				}
				arrowImg.color = Color32.Lerp(new Color32(System.Convert.ToByte(transColor.r * 255),
															System.Convert.ToByte(transColor.g * 255),
															System.Convert.ToByte(transColor.b * 255), 0),
											transColor,
											(elapsedTime - indicator.transitionDuration) / indicator.transitionDuration);
			}
			if (indicator.transition == IndicatorSetting.Transition.Scaling)
			{
				transform.localScale = Vector3.Lerp(Vector3.zero, VR_scale, (elapsedTime - indicator.transitionDuration) / indicator.transitionDuration);
			}
		}

		private void EndFadingValues()
		{
			if (indicator.transition == IndicatorSetting.Transition.Fading)
			{
				if (onScreen)
				{
					transColor = indicator.onScreenColor;
				}
				else
				{
					transColor = indicator.offScreenColor;
				}
				arrowImg.color = transColor;
			}
			if (indicator.transition == IndicatorSetting.Transition.Scaling)
			{
				//scale stuff
				transform.localScale = VR_scale;
			}
		}
	}
}