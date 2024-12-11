using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace CWJ.YU.Mobility
{
	//TODO : 음수값못받게하기
	public class Topic6_2_Solution : MonoBehaviour
	{
		public Ipf_FloatValidator ipf_FloatValidator;

		public GameObject finalMessageObj;
		// 사용자 입력
		public GameObject ipfRootObj;
		bool ipfRootActivated = false;
		public TMP_InputField pxIpf;
		public float px; // x축 끝점 좌표

		public TMP_InputField pyIpf;
		public float py; // y축 끝점 좌표

		public TMP_InputField pzIpf;
		public float pz; // z축 끝점 좌표

		public TMP_InputField d2Ipf;
		public float d2; // 임의의 값 d2

		public Button calculateBtn;

		// 결과
		public TextMeshProUGUI theta11Text;
		public TextMeshProUGUI theta12Text;
		public TextMeshProUGUI theta2Text;
		public TextMeshProUGUI d3Text;

		private void Update()
		{
			if (ipfRootActivated)
			{
				return;
			}

			if (Input.GetKeyDown(KeyCode.Return))
			{
				ipfRootActivated = true;
				ipfRootObj.SetActive(true);
			}
		}

		void Awake()
		{
			ipfRootActivated = false;
			InitValidatorSetting(pxIpf);
			pxIpf.onEndEdit.AddListener((s) =>
			{
				px = float.TryParse(s, out var f) ? f : 0;
				CheckIsPramValid();
			});

			InitValidatorSetting(pyIpf);
			pyIpf.onEndEdit.AddListener((s) =>
			{
				py = float.TryParse(s, out var f) ? f : 0;
				CheckIsPramValid();
			});

			InitValidatorSetting(pzIpf);
			pzIpf.onEndEdit.AddListener((s) =>
			{
				pz = float.TryParse(s, out var f) ? f : 0;
				CheckIsPramValid();
			});

			InitValidatorSetting(d2Ipf);
			d2Ipf.onEndEdit.AddListener((s) =>
			{
				d2 = float.TryParse(s, out var f) ? f : 0;
				CheckIsPramValid();
			});

			calculateBtn.onClick.AddListener(CalculateAllKinematics);
		}

		void InitValidatorSetting(TMP_InputField ipf)
		{
			ipf.contentType = TMP_InputField.ContentType.Custom;
			ipf.lineType = TMP_InputField.LineType.SingleLine;
			ipf.inputType = TMP_InputField.InputType.Standard;
			ipf.characterValidation = TMP_InputField.CharacterValidation.CustomValidator;
			ipf.inputValidator = ipf_FloatValidator;
			ipf.characterLimit = ipf_FloatValidator.inputMaxLength + 1;
			ipf.keyboardType = TouchScreenKeyboardType.DecimalPad;
			ipf.placeholder.GetComponent<TextMeshProUGUI>().SetText($"0보다 큰수 입력");
		}

		void CalculateAllKinematics()
		{
			if (!TryComputeInverseKinematics(px, py, pz, d2, out var theta11, out var theta12, out var theta2, out var d3))
			{
				theta11Text.SetText("음수값은 입력x");
				return;
			}

			float theta11Degrees = theta11 * Mathf.Rad2Deg;
			float theta12Degrees = theta12 * Mathf.Rad2Deg;
			float theta2Degrees = theta2 * Mathf.Rad2Deg;

			theta11Text.SetText($"θ₁₁: {theta11Degrees:F2}°");

			theta12Text.SetText($"θ₁₂: {theta12Degrees:F2}°");

			theta2Text.SetText($"θ₂: {theta2Degrees:F2}°");

			d3Text.SetText($"d₃: {d3:F2}");

			finalMessageObj.SetActive(true);
		}

		private bool CheckIsPramValid()
		{
			bool isValid = _CheckIsPramValid(px, py, pz, d2);
			calculateBtn.interactable = isValid;
			// sqrtTerm 계산
			return isValid;
		}

		private static bool _CheckIsPramValid(float px, float py, float pz, float d2)
		{
			float sqrtTerm = px * px + py * py - d2 * d2;
			return sqrtTerm >= 0;
		}

		public static bool TryComputeInverseKinematics(float px, float py, float pz, float d2, out float _theta11, out float _theta12, out float _theta2,
		                                               out float _d3)
		{
			// sqrtTerm 계산
			float sqrtTerm = px * px + py * py - d2 * d2;

			if (!_CheckIsPramValid(px, py, pz, d2))
			{
				Debug.LogError("오류: 제곱근 내부의 값이 음수입니다. 실수 해가 존재하지 않습니다.");
				_theta11 = _theta12 = _theta2 = _d3 = 0;
				return false;
			}

			float sqrtValue = Mathf.Sqrt(sqrtTerm);

			// 분모
			float denominator = d2 + py;

			// θ₁₁
			float numerator1 = -px + sqrtValue;
			float theta11 = 2 * Mathf.Atan2(numerator1, denominator);
			_theta11 = theta11;

			// θ₁₂
			float numerator2 = -px - sqrtValue;
			float theta12 = 2 * Mathf.Atan2(numerator2, denominator);
			_theta12 = theta12;

			// θ₁ 선택 (여기서는 θ₁₁ 사용)
			float theta1 = theta11;

			// cos(θ₁)과 sin(θ₁) 계산
			float c1 = Mathf.Cos(theta1);
			float s1 = Mathf.Sin(theta1);

			// θ₂
			float numeratorTheta2 = px * c1 + py * s1;
			float theta2 = 2 * Mathf.Atan2(numeratorTheta2, py);
			_theta2 = theta2;

			// d₃
			float d3 = Mathf.Sqrt(numeratorTheta2 * numeratorTheta2 + pz * pz);
			_d3 = d3;
			return true;
		}


		// // 𝜃₁₁ 계산 함수
		// float CalculateTheta11(float px, float py, float d2)
		// {
		// 	float sqrtTerm = Mathf.Sqrt(Mathf.Pow(px, 2) + Mathf.Pow(py, 2) - Mathf.Pow(d2, 2));
		// 	float numerator = -px + sqrtTerm;
		// 	float denominator = d2 + py;
		// 	float theta11Rad = 2 * Mathf.Atan2(numerator, denominator);
		// 	return theta11Rad * Mathf.Rad2Deg;
		// }
		//
		// // 𝜃₁₂ 계산 함수
		// float CalculateTheta12(float px, float py, float d2)
		// {
		// 	float sqrtTerm = Mathf.Sqrt(Mathf.Pow(px, 2) + Mathf.Pow(py, 2) - Mathf.Pow(d2, 2));
		// 	float numerator = -px - sqrtTerm;
		// 	float denominator = d2 + py;
		// 	float theta12Rad = 2 * Mathf.Atan2(numerator, denominator);
		// 	return theta12Rad * Mathf.Rad2Deg;
		// }
		//
		// // 𝜃₂ 계산 함수
		// float CalculateTheta2(float px, float py, float theta1)
		// {
		// 	float theta1Rad = theta1 * Mathf.Deg2Rad;
		// 	float c1 = Mathf.Cos(theta1Rad);
		// 	float s1 = Mathf.Sin(theta1Rad);
		// 	float numerator = px * c1 + py * s1;
		// 	float theta2Rad = 2 * Mathf.Atan2(numerator, py);
		// 	return theta2Rad * Mathf.Rad2Deg;
		// }
		//
		// // d₃ 계산 함수
		// float CalculateD3(float px, float py, float pz, float theta1)
		// {
		// 	float theta1Rad = theta1 * Mathf.Deg2Rad;
		// 	float c1 = Mathf.Cos(theta1Rad);
		// 	float s1 = Mathf.Sin(theta1Rad);
		// 	float d3 = Mathf.Sqrt(Mathf.Pow(px * c1 + py * s1, 2) + Mathf.Pow(pz, 2));
		// 	return d3;
		// }
	}
}
