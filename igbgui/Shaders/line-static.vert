﻿#version 330 core

layout(location = 0) in vec4 position;
layout(location = 1) in vec4 color;

uniform mat4 projectionMatrix;
uniform mat4 viewMatrix;

out vec4 pass_Color;

void main(void)
{
    gl_Position = projectionMatrix * viewMatrix * position.xzyw;
    pass_Color = color;
}
