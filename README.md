#SENEM: A Metaverse Virtual Classroom for the Academic & Educational Context
<h3 align = "center"> Welcome to the Metaverse Classroom! </h3>
<p align = "center"> An open-source project developed with Unity3D and Photon to create a collaborative 3D virtual environment for academic and educational purposes. </p>
<p align = "center">
  <img src = "blobs/presentation_pic.jpg?raw=true" width = "800" heigth = "600">
</p>


## Platform Description
The main goal of this project was to develop and validate a metaverse platform to support academics and students in collaboration and communication tasks.
Concretely, the platform is a large room that implements general tools—inspired by real-world classes—to allow people to collaborate in different scenarios, e.g., conducting or attending lessons, seminars, talks, and working together. 
- **Creation and connection to a room**. The platform offers real-time connection with other users, implemented through a _room system_, allowing users to choose whom to connect with. The first user intending to connect will create the room, providing it with a password, and then share this information with interested individuals.
- **Free three-dimensional exploration**. Freedom of movement and interaction characterize the virtual environment, allowing users to navigate any classroom area and engage with objects seamlessly through their avatar. Users can walk, sit, and interact freely with various objects in the scene. They can rotate their view and adjust the zoom level of their perspective as well.
- **Avatar's customization**. The user has the option to customize their avatar in various ways. The prototype includes a dedicated interface where users can make different appearance choices for their avatar, including skin color, uniform type and color, eye shape and color, eyebrows and possible beard, hairstyle, and a selection of some additional cosmetics like glasses or eye patches.
- **Realistic voice communication**. Users can engage in real-time communication with each other through voice chat. Using their microphones and voices, they can make their avatars speak and hear others as in genuine verbal communication. The voice chat is equipped with proximity and three-dimensionality features, allowing the tone of voice to vary based on the distance and position of the interlocutor. 
- **Text communication**. It is also possible to communicate through text chat, which is readily available and visible in the platform's user interface. 
- **Non-verbal interaction**. Various animations are available for avatars, and users can perform actions such as waving, clapping, or raising their hands.
- **Projection and presentation of multimedia content**. The virtual classroom is equipped with a projector to transmit multimedia content. Users can upload their slides or images and display them during the platform's runtime. The user can navigate the content on the projector with the _presenter_ role. Each user can use the projector to showcase or present their materials and alternate this role with others during the session.
- **Interaction with the whiteboard**. The classroom also has a whiteboard where users can type and write using a keyboard. One user at a time can approach the whiteboard and start writing, and what they write will be visible to everyone, supporting interaction and communication.

<div align="center">
  <img src="blobs/stripe2.JPG?raw=true" width="300" />
  <img src="blobs/stripe3.png?raw=true" width="195" />
  <img src="blobs/stripe4.png?raw=true" width="200" />
  <img src="blobs/stripe5.png?raw=true" width="200" />
</div>


## Content of the Repository
This repository contains:
- _Metaverse_Classroom_2_: The complete Unity Project of the application.
- _Windows Build_: The built application ready to use for Windows.
- _MacOS Build.app_: The built application for MacOS.

## How to Install

### Application only
**Windows:**
1. Download the _Windows Build_ folder.
2. Run the _Metaverse_Classroom_ executable.

**MacOS:**
1. Download the _MacOS Build.app_ and extract it intto a folder
2. Follow these steps to allow your device to run the application:
    - Open the terminal and navigate to the folder that contains the extracted files.
    - Type the following command <code>chmod -R +x MacOS Build.app/Contents/MacOS</code> into the terminal.
    - Type the following command <code>xattr -cr MacOS Build.app</code> into the terminal.
    - Double-click on _MacOS Build.app_.

### Unity Project

**Requirements:**
- Unity3D version: 2021.3.22f1
- Unity Hub

**Steps:**
1. Download _Metaverse_Classroom_2_.
2. Open the Unity Hub, go in the _Projects_ tab and click on the _Open_ button.
3. Choose the path where you downloaded the _Metaverse_Classroom_2_.
4. The project should now appear in your projects list. Click on its name and Unity will install all the needed libraries to make it work.

## Author
* **Viviana Pentangelo** - [vipenti](https://github.com/vipenti)
