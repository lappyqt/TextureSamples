#version 330 core

in vec2 texCoord;

out vec4 outputColor;

uniform sampler2D texture0;
uniform sampler2D texture1;
uniform float alpha;

void main() {
    outputColor = mix(texture(texture0, texCoord), texture(texture1, texCoord), alpha);
}