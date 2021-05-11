# Shape Song

Shape song was developed as the core application for my graduate school master's research. It was built from the ground up in the Unity game engine (https://unity.com/), it uses Audiokinetic's Wwise as the sound engine (https://www.audiokinetic.com/products/wwise/), and it uses the Photon Engine to handle realtime networking (https://www.photonengine.com/). See the poster for a screenshot of the application.

## Keyboard Controls
- Move your character with the WASD keys.
- Press "J" to create a note

## Creating Music
### The Stick Figure
- When you press J, your horizontal position on the screen determines the note volume and your vertical position on the screen determines your note pitch.
- The character color sliders on the left hand side of the screen will control both the stick figure color and parameters of the synthesizer played with the "J" key.
- You can control other musicians' colors and synthesizer parameters by selecting a name on the side of the screen. This will link the color sliders to the appropriate stick figure.

### Shapes
- Up to 8 squares can be created at a time. Each is associated with a percussion instrument/sound. These are kick drum, snare drum, hi hat, low tom, mid tom, high tom, cowbell, and clap. Squares are linked to the percussion instruments in the order listed prior, and they can be deleted in the reverse order. 
- Similarly, up to 3 circles can be created at a time. Each is associated with a background melodic line organized into low (bass), mid, and high. Like squares, circles are linked to the background melodic lines starting with low and ending with high. Circles are deleted in the reverse order that they were created. 
- The shapes can be double clicked to be selected. The shape color and shape brightness sliders can be used to change the color and brightness of whatever shape is selected. Color is associated with different percussive/melodic patterns whereas brightness is associated with volume. 

### Intensity
- The intensity slider controls the brightness of the background as well as a low pass filter on all audio.
