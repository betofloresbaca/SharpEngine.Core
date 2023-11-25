﻿using System;
using SharpEngine.Core.Math;
using SharpEngine.Core.Renderer;

namespace SharpEngine.Core.Component;

/// <summary>
/// Component which display sprite
/// </summary>
/// <param name="texture">Name Texture Displayed</param>
/// <param name="displayed">If Texture is Displayed (true)</param>
/// <param name="offset">Offset (Vec2(0))</param>
/// <param name="flipX">If Sprite is Flip Horizontally</param>
/// <param name="flipY">If Sprite is Flip Vertically</param>
/// <param name="zLayerOffset">Offset of zLayer</param>
/// <param name="shader">Sprite Shader ("")</param>
public class SpriteComponent(
    string texture,
    bool displayed = true,
    Vec2? offset = null,
    bool flipX = false,
    bool flipY = false,
    int zLayerOffset = 0,
    string shader = ""
) : Component
{
    /// <summary>
    /// Name of Texture which be displayed
    /// </summary>
    public string Texture { get; set; } = texture;

    /// <summary>
    /// Define if Sprite is displayed
    /// </summary>
    public bool Displayed { get; set; } = displayed;

    /// <summary>
    /// Offset of Sprite
    /// </summary>
    public Vec2 Offset { get; set; } = offset ?? Vec2.Zero;

    /// <summary>
    /// If Sprite is Flip Horizontally
    /// </summary>
    public bool FlipX { get; set; } = flipX;

    /// <summary>
    /// If Sprite is Flip Vertically
    /// </summary>
    public bool FlipY { get; set; } = flipY;

    /// <summary>
    /// Offset of ZLayer of Sprite
    /// </summary>
    public int ZLayerOffset { get; set; } = zLayerOffset;

    /// <summary>
    /// Shader of Sprite
    /// </summary>
    public string Shader { get; set; } = shader;

    private TransformComponent? _transformComponent;

    /// <inheritdoc />
    public override void Load()
    {
        base.Load();

        _transformComponent = Entity?.GetComponentAs<TransformComponent>();
    }

    /// <inheritdoc />
    public override void Draw()
    {
        base.Draw();

        var window = Entity?.Scene?.Window;

        if (_transformComponent == null || !Displayed || Texture.Length <= 0 || window == null)
            return;

        var texture = window.TextureManager.GetTexture(Texture);
        var position = _transformComponent.GetTransformedPosition(Offset);
        if (Shader == "")
        {
            SERender.DrawTexture(
                texture,
                new Rect(
                    0,
                    0,
                    FlipX ? -texture.Width : texture.Width,
                    FlipY ? -texture.Height : texture.Height
                ),
                new Rect(
                    position.X,
                    position.Y,
                    texture.Width * _transformComponent.Scale.X,
                    texture.Height * _transformComponent.Scale.Y
                ),
                new Vec2(
                    texture.Width / 2f * _transformComponent.Scale.X,
                    texture.Height / 2f * _transformComponent.Scale.Y
                ),
                _transformComponent.Rotation,
                Utils.Color.White,
                InstructionSource.Entity,
                _transformComponent.ZLayer + ZLayerOffset
            );
        }
        else
        {
            SERender.ShaderMode(
                window.ShaderManager.GetShader(Shader),
                InstructionSource.Entity,
                _transformComponent.ZLayer + ZLayerOffset,
                () =>
                {
                    SERender.DrawTexture(
                        texture,
                        new Rect(
                            0,
                            0,
                            FlipX ? -texture.Width : texture.Width,
                            FlipY ? -texture.Height : texture.Height
                        ),
                        new Rect(
                            position.X,
                            position.Y,
                            texture.Width * _transformComponent.Scale.X,
                            texture.Height * _transformComponent.Scale.Y
                        ),
                        new Vec2(
                            texture.Width / 2f * _transformComponent.Scale.X,
                            texture.Height / 2f * _transformComponent.Scale.Y
                        ),
                        _transformComponent.Rotation,
                        Utils.Color.White,
                        InstructionSource.Entity,
                        _transformComponent.ZLayer + ZLayerOffset
                    );
                }
            );
        }
    }
}
