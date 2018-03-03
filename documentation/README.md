# Content Explorer Module Documentation

## Module Purpose

Our **Content Explorer** module is intended to enable editors have alternative Sitemap view where sitecore items are represented by sitemap tree nodes. Content editors can further explore components available for each page item and preview content of the renderings datasource associated with renderings.

**Content Explorer** module is built using JSS headless services and emphasizes JSS capabilities and customization options.

Our module will be extremely helpful for clients that use Sitecore headless to populate content in Mobile apps and alternative channels that don't have web view, i.e. where content cannot be previewed in Sitecore Experience Editor.

## Module Sitecore Hackathon Category

Our category is **JSS**

## Pre-requisites

* Sitecore 9.0.1 rev. 171219
* Sitecore JSS 7.0 for Sitecore 9.0 (Tech Preview 2)

## Installation Process

* Install Sitecore 9.0 Update 1
* Download and install Server Package (Tech Preview 9.0.1 rev. 180228).zip (you can find package here: https://dev.sitecore.net/Downloads/Sitecore_JavaScript_Services/90_Tech_Preview/Sitecore_JavaScript_Services_90_Tech_Preview_2.aspx)
* Install Fast Turtles-1.1.zip (you can find package here: \sc.package\Fast Turtles-1.1.zip)
* Go to Launchpad -> Fast Turtles > Content Explorer

## Usage

Once installed content editor will get new **Content Explorer** app added to the Sitecore launch pad

![sitecore launchpad](https://user-images.githubusercontent.com/16732500/36940354-93b60492-1f52-11e8-8962-c3f24cecc761.png)

Browsing to the **Content Explorer**, editor can see complete sitemap. Each node in the sitemap is represented by sitecore icon,  item name and workflow status. Editor can easily see levels identified by different colors

![content explorer](https://user-images.githubusercontent.com/16732500/36940395-feed8e9c-1f52-11e8-974b-4a66bc4756e4.png)

**Content editor** can dblclick on the sitemap node to see renderings assigned to each node. All these data we are retrieving from JSS Layout Service

![content explorer - item details](https://user-images.githubusercontent.com/16732500/36940421-74f2573a-1f53-11e8-9470-f0dead0cb073.png)

## Video
Our video: [direct link](https://www.screencast.com/t/ij4r3RXpb)
Please provide a video highlighing your Hackathon module submission and provide a link to the video. Either a [direct link](C:\Users\apr.BRIMIT\Desktop\2018-03-04_0206.swf) to the video, upload it to this documentation folder or maybe upload it to Youtube...

[![Sitecore Hackathon Video Embedding Alt Text](https://img.youtube.com/vi/EpNhxW4pNKk/0.jpg)](https://www.youtube.com/watch?v=EpNhxW4pNKk)
