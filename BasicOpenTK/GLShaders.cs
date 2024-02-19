using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicOpenTK
{
    internal static class GLShaders
    {
        public static string VertexLightCudeShader { get { return vertexLightCube; } }
        public static string PixelLightCubeShader { get { return pixelLightCube; } }

        public static string VertexCodeShader { get { return vertexShaderCode; } }
        public static string PixelCodeShader { get { return pixelShaderCode; } }

        static string vertexLightCube =
                @"
                #version 330 core
                layout (location = 0) in vec3 aPos;

                uniform mat4 Model;
                uniform mat4 View;
                uniform mat4 Projection;

                void main()
                {
	                gl_Position = Projection * View * Model * vec4(aPos, 1.0);
                }
                ";
        static string pixelLightCube =
            @"
                #version 330 core
                out vec4 FragColor;

                void main()
                {
                    FragColor = vec4(1.0);
                }
                ";

        static string vertexShaderCode =
            @"
                #version 330 core

                out vec3 FragPos;
                out vec3 Normal;

                uniform float ColorFactor;
                uniform mat4 Projection;
                uniform mat4 View;
                uniform mat4 Model;

                layout (location = 0) in vec3 aPosition;
                layout (location = 1) in vec4 aColor;
                layout (location = 2) in vec4 aNormal;

                out vec4 vColor;

                void main()
                {
                    vColor = aColor * ColorFactor;

                    FragPos = vec3(Model * vec4(aPosition, 1.0));
                    Normal = mat3(transpose(inverse(Model))) * vec3(aNormal.x, aNormal.y, aNormal.z);

                    gl_Position = Projection * View * vec4(FragPos, 1.0);
                }
                ";

        static string pixelShaderCode =
            @"
                 #version 330 core
                struct Material {
                    vec3 ambient;
                    vec3 diffuse;
                    vec3 specular;
                    float shininess;
                }; 
  
                uniform Material material;
                struct Light {
                    vec3 position;
  
                    vec3 ambient;
                    vec3 diffuse;
                    vec3 specular;
                };

                uniform Light light;  

                 in vec3 Normal;  
                 in vec3 FragPos;

                 in vec4 vColor;
                 out vec4 pixelColor;

                 //uniform vec3 lightPos;
                 uniform vec3 viewPos; 
                 uniform vec3 color;
                 //uniform vec3 lightColor;

                 void main(){
                    // ambient
                    //float ambientStrength = 0.2;
                    vec3 ambient = light.ambient * material.ambient;
  	
                    // diffuse 
                    vec3 norm = normalize(Normal);
                    vec3 lightDir = normalize(light.position - FragPos);
                    float diff = max(dot(norm, lightDir), 0.0);
                    vec3 diffuse = light.diffuse * diff * material.diffuse;
            
                    // specular
                    //float specularStrength = 1.5;
                    vec3 viewDir = normalize(viewPos - FragPos);
                    vec3 reflectDir = reflect(-lightDir, norm);  
                    float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shininess);
                    vec3 specular = light.specular * spec * material.specular;  
                    vec3 resCol = vec3(vColor.x, vColor.y, vColor.z);
                    
                    if (color.x != 0 || color.y != 0 || color.z != 0) 
                        resCol = color;
                    
                    vec3 result = (ambient + diffuse + specular) * resCol;
                    pixelColor = vec4(result, 1.0);
                 }
                 ";
    }
}
