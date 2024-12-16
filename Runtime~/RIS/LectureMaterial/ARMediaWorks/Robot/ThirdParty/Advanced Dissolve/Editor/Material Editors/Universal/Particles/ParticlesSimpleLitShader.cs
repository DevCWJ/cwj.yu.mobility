using System;
using System.Collections.Generic;

using UnityEngine;

using AmazingAssets.AdvancedDissolveEditor;


namespace UnityEditor.Rendering.Universal.ShaderGUI
{
    internal class AdvancedDissolve_ParticlesSimpleLitShader : BaseShaderGUI
    {
        // Properties
        private SimpleLitGUI.SimpleLitProperties shadingModelProperties;
        private ParticleGUI.ParticleProperties particleProps;

        // List of renderers using this material in the scene, used for validating vertex streams
        List<ParticleSystemRenderer> m_RenderersUsingThisMaterial = new List<ParticleSystemRenderer>();

        //Advanced Dissolve
        MaterialHeaderScopeList curvedWorldMaterialScope;
        MaterialHeaderScopeList advancedDissolveMaterialScope;

        public override void FillAdditionalFoldouts(MaterialHeaderScopeList materialScopesList)
        {
            base.FillAdditionalFoldouts(materialScopesList);

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

        public override void FindProperties(MaterialProperty[] properties)
        {
            base.FindProperties(properties);
            shadingModelProperties = new SimpleLitGUI.SimpleLitProperties(properties);
            particleProps = new ParticleGUI.ParticleProperties(properties);

            //Advanced Dissolve
            AdvancedDissolveMaterialProperties.Init(properties);
        }

        public override void ValidateMaterial(Material material)
        {
            SetMaterialKeywords(material, SimpleLitGUI.SetMaterialKeywords, ParticleGUI.SetMaterialKeywords);

            //Advanced Dissolve
            AdvancedDissolveMaterialProperties.SetMaterialKeywords(material);
        }

        public override void DrawSurfaceOptions(Material material)
        {
                base.DrawSurfaceOptions(material);
                DoPopup(ParticleGUI.Styles.colorMode, particleProps.colorMode, Enum.GetNames(typeof(ParticleGUI.ColorMode)));
            }

        public override void DrawSurfaceInputs(Material material)
        {
            base.DrawSurfaceInputs(material);
            SimpleLitGUI.Inputs(shadingModelProperties, materialEditor, material);
            DrawEmissionProperties(material, true);
        }

        public override void DrawAdvancedOptions(Material material)
        {
            SimpleLitGUI.Advanced(shadingModelProperties);

                materialEditor.ShaderProperty(particleProps.flipbookMode, ParticleGUI.Styles.flipbookMode);
                ParticleGUI.FadingOptions(material, materialEditor, particleProps);
                ParticleGUI.DoVertexStreamsArea(material, m_RenderersUsingThisMaterial, true);

            DrawQueueOffsetField();
        }

        public override void OnOpenGUI(Material material, MaterialEditor materialEditor)
        {
            CacheRenderersUsingThisMaterial(material);
            base.OnOpenGUI(material, materialEditor);
        }

        void CacheRenderersUsingThisMaterial(Material material)
        {
            m_RenderersUsingThisMaterial.Clear();

            ParticleSystemRenderer[] renderers = UnityEngine.Object.FindObjectsOfType(typeof(ParticleSystemRenderer)) as ParticleSystemRenderer[];
            foreach (ParticleSystemRenderer renderer in renderers)
            {
                if (renderer.sharedMaterial == material)
                    m_RenderersUsingThisMaterial.Add(renderer);
            }
        }
    }
} // namespace UnityEditor
