Unique Lasers Volume 1 - Readme


This package contains:

- 14 Laser Prefabs (customizable);
- Lasers can Bounce / Reflect  when colliding;
- Laser Beam Script (control color, bounces, size, fire point, end point, max length, overgrow etc);
- FPS and TPS PC Demo;
- FPS and TPS ​Mobile Demo;
- Pack of Textures for customization;
- Customizable Shaders;
- Particle System Controller Script (control size, speed, color, lights, trails, enable/disable vfxs, etc).



TPS_SCENE & FPS_SCENE - SHORTCUTS:

W S A D - Move in FPS
Mouse 1 - Fire Laser
E - Next Effect
Q - Previous Effect
R/F - Increase/Decrease Bounces
X/Z - Increase/Decrease Size



LASER SCRIPT - DESCRIPTION:

Options:
'Change Color' - If this is On, it will apply the colors in 'New Max Color' and 'New Min Color'. If off it goes back to original.
'New Max Color' - Maximum color if you want a range of colors.
'New Min Color' - Minimum color useful for when creating a range. Use same color as 'New Max Color' if you don't want any color range.
'Line Renderers' - This is the core of the Lasers. It's a list of Line Renderers.
'Fire Point' - The starting position of the Laser.
'End Point' - The end position of the Laser. If using an input to control the endpoint, like the mouse for example, then assign the camera.
'Bounces' - How many times the Laser bounces when it collides.
'Size' - Multiplies the original size of the Laser (eg: 0.5 - half the size / 2 - double the size).
'Maximum Length' - The maximum length the Laser will have.
'Overgrow' - How much it overgrows when the Laser is fired.
'Grow Speed' - The speed which the overgrow grows.
'Shrink Speed' - How fast it shrinks when we stop shooting.
'Disable Delay' - How many seconds before the Laser is disabled.
'Reflect Start' - When using bounces, you can choose if you want to duplicate the Start VFX.
'Start VFX' - Parent of all the particles systems at the start of the Laser.
'Reflect Middle' - When using bounces, you can choose if you want to duplicate the Middle VFX.
'Middle VFX' - Parent of all the particles systems at the middle of the Laser.
'Reflect End' - When using bounces, you can choose if you want to duplicate the End VFX.
'End VFX' - Parent of all the particle systems at the end of the Laser.
'Trail' - Enable or disable trail.
'Reflect Trail' - When using bounces, you can choose if you want to duplicate the Trail.
'Trail VFX' - The particle system that leaves a trail.
'Trail Interval' - The interval between each trail - 0 means a continuous trail.




PARTICLE SYSTEM CONTROLLER SCRIPT - DESCRIPTION:

Options:
'Size' - Multiplies Particle Systems and Trails sizes.
'Speed' - Multiplies Particle Systems and Trails speeds.
'Loop' - Enable/Disable Particle Systems loop.
'Lights' - Enable/Disable Particle Systems lights.
'Trails' - Enable/Disable Particle Systems trails.
'Changes Color' - Enable/Disable changing color of Particle Systems and Trails speeds.
'New Max Color' - New maximum color.
'New Min Color' - New minimum color.
'Particle Systems' - The Particle Systems and Trails the prefab contains. Can be filled automatically with 'Fill Lists' button, or manually.
'Active Particle Systems' - Choose which Particle Systems and Trails are active. Can be filled automatically with 'Fill Lists' button, or manually.
'Fill Lists' - Finds and adds Particle Systems and Trails, of the parent and childs of current gameobject, to 'Particle Systems' and 'Active Particle Systems' lists.
'Empty Lists' - Emptys 'Particle Systems' and 'Active Particle Systems' lists.
'Apply' - It will apply the changes you made (Size, Speed, Loop, Lights Enabled/Disabled, Trails Enabled/Disabled, Change Color) to the particle systems in 'Particle Systems' that ARE active in the 'Active Particle Systems' list. It will also save the original settings in a folder called 'Original Settings' inside the folder of the vfx prefab.
'Reset' - Resets the Particle Systems and Trails to the original settings which are saved in a folder called 'Original Settings' inside the folder of the vfx prefab.

Workflow:
1) Add script to any VFX prefab;
2) Press 'Fill Lists' to automatically find and add Particle Systems and Trails to lists;
3) Make your changes (Size, Speed, Loop Enabled/Disabled, Lights Enabled/Disabled, Trails Enabled/Disabled, Change Color, Enable/Disable Particle Systems with 'Active Particle Systems' lists);
4) Press 'Aplly';
5) Script saves original settings and applies changes;
6) That's it, enjoy. 
PS: You can always press 'Reset' to go back to original settings.

Warnings:
1) Don't change the name of the VFX after you have pressed 'Apply'. Otherwise 'Reset' will not work since it wouldn't be able to find the original settings.
2) You can change the name of the VFX BUT you must go to the respective 'Original Settings' folder and copy paste the exact same name of the VFX. Watch video for a demonstration.
3) 'Fill Lists' won't work if the gameobject contains a Particle System and a Trail (at least for now)




MOBILE IMPROVEMENTS:

- I recommend you deactive distortion, trails, lights and even the dark part of the laser. Those are the main GPU hungry factors of this effects;

- Use the least amount of particle systems possible (by turning off with the script "ParticleSystemController";



POST-PROCESSING EFFECTS INFO:

All footage was done using Post-Processing Effects. Follow this steps if you want to achieve that quality:

1) Go to Unity Archives ( https://unity3d.com/get-unity/download/archive ), select your version and download the Standard Assets from the drop-down list;

2) Import that package to your project in Unity (you can only import the Effects folder if you want);

3) In the DemoScene01 you can add "Depth Of Field", "Bloom", "Vignetter And Chromatic Aberration", "Sun Shafts" and "Color Correction Curves" components to the cameras;

4) Copy values from screenshot "PostProcessingParameters";

4) Enjoy and Have Fun!



CONTACTS:

Feel free to contact me via links bellow in case you have any doubts. 

Twitter: @GabrielAguiProd

Facebook: facebook.com/gabrielaguiarprod/

YouTube: youtube.com/c/gabrielaguiarprod



Thank you for purchasing the Unique Lasers Volume 1 package.
Unique Lasers Volume 01 is created by Gabriel Aguiar




Any feedback on the Asset Store is very Welcome!

