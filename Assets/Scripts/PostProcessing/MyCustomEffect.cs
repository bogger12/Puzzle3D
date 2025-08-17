using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[Serializable]
[PostProcess(typeof(MyCustomEffectRenderer), PostProcessEvent.AfterStack, "Custom/MyCustomEffect")]
public sealed class MyCustomEffect : PostProcessEffectSettings
{
  [Range(0f, 1f), Tooltip("MyCustomEffect effect intensity.")]
   public FloatParameter blend = new FloatParameter { value = 0.5f };
}
public sealed class MyCustomEffectRenderer : PostProcessEffectRenderer<MyCustomEffect>
{
   public override void Render(PostProcessRenderContext context)
  {
       var sheet = context.propertySheets.Get(Shader.Find("Hidden/MyCustomEffect"));
       sheet.properties.SetFloat("_Blend", settings.blend);
       context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
  }
}
