using System;

using UnityEngine;

using AmazingAssets.AdvancedDissolveEditor;


namespace UnityEditor.Rendering.Universal.ShaderGUI
{
    internal class AdvancedDissolve_LitShader : BaseShaderGUI
    {
        static readonly string[] workflowModeNames = Enum.GetNames(typeof(LitGUI.WorkflowMode));

        private LitGUI.LitProperties litProperties;
        private AdvancedDissolve_LitDetailGUI.LitProperties litDetailProperties;



        //Advanced Dissolve
        MaterialHeaderScopeList curvedWorldMaterialScope;
        MaterialHeaderScopeList advancedDissolveMaterialScope;


        public override void FillAdditionalFoldouts(MaterialHeaderScopeList materialScopesList)
        {
            materialScopesList.RegisterHeaderScope(AdvancedDissolve_LitDetailGUI.Styles.detailInputs, Expandable.Details, _ => AdvancedDissolve_LitDetailGUI.DoDetailArea(litDetailProperties, materialEditor));



            Material material = (Material)materialEditor.target;
            if (curvedWorldMaterialScope == null)
                curvedWorldMaterialScope = new MaterialHeaderScopeList();
            if (material.HasProperty("_CurvedWorldBendSettings"))
                curvedWorldMaterialScope.RegisterHeaderScope(AdvancedDissolveMaterialProperties.Styles.curvedWorldHeader, AdvancedDissolveMaterialProperties.Expandable.CurvedWorld, _ => AdvancedDissolveMaterialProperties.DrawCurvedWorldHeader(false, GUIStyle.none, materialEditor, material));

            if (advancedDissolveMaterialScope == null)
                advancedDissolveMaterialScope = new MaterialHeaderScopeList();
            advancedDissolveMaterialScope.RegisterHeaderScope(AdvancedDissolveMaterialProperties.Styles.beastHeader, AdvancedDissolveMaterialProperties.Expandable.AdvancedDissolve, _ => AdvancedDissolveMaterialProperties.DrawDissolveOptions(false, materialEditor, false, false, true, true, true));
        }

        public override void OnGUI(MaterialEditor materialEditorIn, MaterialProperty[] properties)
        {
            if (materialEditorIn == null)
                throw new ArgumentNullException("materialEditorIn");

            materialEditor = materialEditorIn;
            Material material = materialEditor.target as Material;

            FindProperties(properties);   // MaterialProperties can be animated so we do not cache them but fetch them every event to ensure animated values are updated correctly

            // Make sure that needed setup (ie keywords/renderqueue) are set up if we're switching some existing
            // material to a universal shader.
            if (m_FirstTimeApply)
            {
                OnOpenGUI(material, materialEditorIn);
                m_FirstTimeApply = false;
            }

            //Curved World
            curvedWorldMaterialScope.DrawHeaders(materialEditor, material);

            ShaderPropertiesGUI(material);

            //Advanced Dissolve
            advancedDissolveMaterialScope.DrawHeaders(materialEditor, material);
        }

        // collect properties from the material properties
        public override void FindProperties(MaterialProperty[] properties)
        {
            base.FindProperties(properties);
            litProperties = new LitGUI.LitProperties(properties);
            litDetailProperties = new AdvancedDissolve_LitDetailGUI.LitProperties(properties);

            //Advanced Dissolve
            AdvancedDissolveMaterialProperties.Init(properties);
        }

        // material changed check
        public override void ValidateMaterial(Material material)
        {
            SetMaterialKeywords(material, LitGUI.SetMaterialKeywords, AdvancedDissolve_LitDetailGUI.SetMaterialKeywords);

            //Advanced Dissolve
            AdvancedDissolveMaterialProperties.SetMaterialKeywords(material);
        }

        // material main surface options
        public override void DrawSurfaceOptions(Material material)
        {
            // Use default labelWidth
            EditorGUIUtility.labelWidth = 0f;

            if (litProperties.workflowMode != null)
                DoPopup(LitGUI.Styles.workflowModeText, litProperties.workflowMode, workflowModeNames);

            
            base.DrawSurfaceOptions(material);            
        }

        // material main surface inputs
        public override void DrawSurfaceInputs(Material material)
        {
            base.DrawSurfaceInputs(material);
            LitGUI.Inputs(litProperties, materialEditor, material);
            DrawEmissionProperties(material, true);
            DrawTileOffset(materialEditor, baseMapProp);
        }

        // material main advanced options
        public override void DrawAdvancedOptions(Material material)
        {
            if (litProperties.reflections != null && litProperties.highlights != null)
            {
                materialEditor.ShaderProperty(litProperties.highlights, LitGUI.Styles.highlightsText);
                materialEditor.ShaderProperty(litProperties.reflections, LitGUI.Styles.reflectionsText);
            }

            base.DrawAdvancedOptions(material);
        }

        public override void AssignNewShaderToMaterial(Material material, Shader oldShader, Shader newShader)
        {
            if (material == null)
                throw new ArgumentNullException("material");

            // _Emission property is lost after assigning Standard shader to the material
            // thus transfer it before assigning the new shader
            if (material.HasProperty("_Emission"))
            {
                material.SetColor("_EmissionColor", material.GetColor("_Emission"));
            }

            base.AssignNewShaderToMaterial(material, oldShader, newShader);

            if (oldShader == null || !oldShader.name.Contains("Legacy Shaders/"))
            {
                SetupMaterialBlendMode(material);
                return;
            }

            SurfaceType surfaceType = SurfaceType.Opaque;
            BlendMode blendMode = BlendMode.Alpha;
            if (oldShader.name.Contains("/Transparent/Cutout/"))
            {
                surfaceType = SurfaceType.Opaque;
                material.SetFloat("_AlphaClip", 1);
            }
            else if (oldShader.name.Contains("/Transparent/"))
            {
                // NOTE: legacy shaders did not provide physically based transparency
                // therefore Fade mode
                surfaceType = SurfaceType.Transparent;
                blendMode = BlendMode.Alpha;
            }
            material.SetFloat("_Blend", (float)blendMode);

            material.SetFloat("_Surface", (float)surfaceType);
            if (surfaceType == SurfaceType.Opaque)
            {
                material.DisableKeyword("_SURFACE_TYPE_TRANSPARENT");
            }
            else
            {
                material.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
            }

            if (oldShader.name.Equals("Standard (Specular setup)"))
            {
                material.SetFloat("_WorkflowMode", (float)LitGUI.WorkflowMode.Specular);
                Texture texture = material.GetTexture("_SpecGlossMap");
                if (texture != null)
                    material.SetTexture("_MetallicSpecGlossMap", texture);
            }
            else
            {
                material.SetFloat("_WorkflowMode", (float)LitGUI.WorkflowMode.Metallic);
                Texture texture = material.GetTexture("_MetallicGlossMap");
                if (texture != null)
                    material.SetTexture("_MetallicSpecGlossMap", texture);
            }
        }
    }
}
