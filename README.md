# SENEM-AI: Smart Student Edition
<p align = "center"> <img src = "blobs/SENEM_logo.png?raw=true" width = "100px"> </p>
<h3 align = "center"> Welcome to the SENEM-AI! </h3>
<p align = "center"> An open-source project developed with Unity3D and Photon to create a collaborative 3D virtual environment for academic and educational purposes.</p>
<p align="center"><i>Now powered by AI!</i></p>
<p align = "center">
  <img src = "blobs/senem-ai.png?raw=true" width = "100%">
</p>

---
## Platform Description
SENEM-AI is a learning virtual environment for lecturers and studeents. To learn more about the SENEM project, [check our paper](https://github.com/vipenti/SENEM_Metaverse/blob/main/SENEM_Paper.pdf).
- **Room system**: Real-time connection with other users, by creating or joining rooms.
- **3D exploration**: Navigate the classroom freely, interact with objects, walk, sit, rotate view, and adjust zoom.
- **Avatar customization**: Personalize appearance, including skin, uniform, eyes, eyebrows, hair, beard, glasses, and other cosmetics.
- **Voice chat**: Realistic communication with proximity and 3D audio effects.
- **Text chat**: Integrated text communication available in the interface.
- **Non-verbal actions**: Perform animations like waving, clapping, or raising hands.
- **Multimedia projection**: Upload and display slides/images via a projector; control content with a presenter role.
- **Whiteboard interaction**: Type and write on a shared whiteboard; visible to all participants, with one user at a time writing.

<div align="center">
  <img src="blobs/stripe2.JPG?raw=true" width="280" />
  <img src="blobs/stripe3.png?raw=true" width="175" />
  <img src="blobs/stripe4.png?raw=true" width="180" />
  <img src="blobs/stripe5.png?raw=true" width="180" />
</div>

## _New:_ Smart Students are here!
SENEM now features Smart Students, powered by LLMs! These AI-driven avatars simulate realistic student behaviors to help presenters and lecturers to train their presentation and question answering skills! 
Choose a topic lesson and the number of students you want, and you will find thm in your virtual classroom.
To make them work in SENEM, you will need to get the [Smart Student Server](https://github.com/vipenti/Smart_Student_Server).
- **Different personalities, voices, and appearances**: Each Smart Student features unique appearances, personality parameters that shape their interaction style, and randomized voices to simulate a diverse classroom audience.  
- **Vocal and textual interaction**: They communicate through a voice synthesizer and actively participate in the text chat by asking or answering questions.  
- **Make and answer questions**: Smart Students can engage dynamically by responding to your questions or posing their own as the lesson unfolds.  

<p align = "center"> <img src = "blobs/smart_students.png?raw=true" width = "100%"> </p>

---
## Content of the Repository
This repository contains:
- _Metaverse_Classroom_2_: The complete Unity Project of the application.
- _Windows Build_: The built application ready to use for Windows.
- _MacOS Build.app_: The built application for MacOS.

---

## How to Install
You can either use a pre-built version of the application or access the entire project in Unity. In either case, if you want to try out the Smart Students functionality you will need the [corresponding server](https://github.com/vipenti/Smart_Student_Server) and follow the instructions there for installation.

### Application only
**Windows:**
1. Download the _Windows Build_ folder.
2. Run the _SENEM_AI_ executable.

**MacOS:**
1. **Download and Extract**  
   - Download the file `_MacOS Build.app_` and extract it into a folder.

2. **Allow Your Device to Run the Application**  
   Follow these steps to grant the necessary permissions:  
   - Open the Terminal and navigate to the folder containing the extracted files.  
   - Run the following commands in the Terminal:  
     ```bash
     chmod -R +x "MacOS Build.app/Contents/MacOS"
     xattr -cr "MacOS Build.app"
     ```  
   - Double-click on `_MacOS Build.app_` to launch the application.

### Unity Project
**Requirements:**
- Unity3D version: 2021.3.22f1
- Unity Hub
- <a href="https://github.com/vipenti/Smart_Student_Server">Smart Student Server (optional, runs locally)</a>

**Steps:**
1. Download _Metaverse_Classroom_2_.
2. Open the Unity Hub, go in the _Projects_ tab and click on the _Add_ button.
3. Choose the path where you downloaded the _Metaverse_Classroom_2_.
4. The project should now appear in your projects list. Click on its name and Unity will install all the needed libraries to make it work.
5. [Optional] Clone the Smart Students Server repository and follow [the instructions](https://github.com/vipenti/Smart_Student_Server/blob/main/README.md) to install it.

---
Have fun with SENEM-AI!
## Authors
* **Viviana Pentangelo** - [vipenti](https://github.com/vipenti)
* **Luigi Turco** - [KronosPNG](https://github.com/KronosPNG)
