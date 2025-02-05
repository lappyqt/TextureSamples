#version 330 core

in vec2 texCoord;

out vec4 outputColor;

uniform sampler2D texture0;

void main() {
    outputColor = texture(texture0, texCoord);
}