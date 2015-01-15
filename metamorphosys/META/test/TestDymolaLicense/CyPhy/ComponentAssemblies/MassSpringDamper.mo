/*
Copyright (C) 2013-2015 MetaMorph Software, Inc

Permission is hereby granted, free of charge, to any person obtaining a
copy of this data, including any software or models in source or binary
form, as well as any drawings, specifications, and documentation
(collectively "the Data"), to deal in the Data without restriction,
including without limitation the rights to use, copy, modify, merge,
publish, distribute, sublicense, and/or sell copies of the Data, and to
permit persons to whom the Data is furnished to do so, subject to the
following conditions:

The above copyright notice and this permission notice shall be included
in all copies or substantial portions of the Data.

THE DATA IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
THE AUTHORS, SPONSORS, DEVELOPERS, CONTRIBUTORS, OR COPYRIGHT HOLDERS BE
LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
WITH THE DATA OR THE USE OR OTHER DEALINGS IN THE DATA.  

=======================
This version of the META tools is a fork of an original version produced
by Vanderbilt University's Institute for Software Integrated Systems (ISIS).
Their license statement:

Copyright (C) 2011-2014 Vanderbilt University

Developed with the sponsorship of the Defense Advanced Research Projects
Agency (DARPA) and delivered to the U.S. Government with Unlimited Rights
as defined in DFARS 252.227-7013.

Permission is hereby granted, free of charge, to any person obtaining a
copy of this data, including any software or models in source or binary
form, as well as any drawings, specifications, and documentation
(collectively "the Data"), to deal in the Data without restriction,
including without limitation the rights to use, copy, modify, merge,
publish, distribute, sublicense, and/or sell copies of the Data, and to
permit persons to whom the Data is furnished to do so, subject to the
following conditions:

The above copyright notice and this permission notice shall be included
in all copies or substantial portions of the Data.

THE DATA IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
THE AUTHORS, SPONSORS, DEVELOPERS, CONTRIBUTORS, OR COPYRIGHT HOLDERS BE
LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
WITH THE DATA OR THE USE OR OTHER DEALINGS IN THE DATA.  
*/

within CyPhy.ComponentAssemblies;
model MassSpringDamper
  //Parameters
  parameter Real mass_start_acc=10;

  //Metrics
  //Environments

  //ComponentAssemblies

  //Components
  replaceable
    CyPhy.Components.MSD.Components.Dampers.Damper_1
    Damper_1__Damper_1(
d=1) constrainedby
    Modelica.Mechanics.Translational.Components.Damper
    annotation(choicesAllMatching=true, Placement(transformation(origin={15,-15}, extent={{-15,-15},{15,15}})));
  replaceable
    CyPhy.Components.MSD.Components.Masses.Mass_5
    Mass_1__Mass_5(
    __CyPhy__a_start=mass_start_acc,
    massa=5) constrainedby
    ModifiedMass.MassInitial
    annotation(choicesAllMatching=true, Placement(transformation(origin={15,-15}, extent={{-15,-15},{15,15}})));
  replaceable
    CyPhy.Components.MSD.Components.Springs.Spring_1
    Spring_1__Spring_1(
c=1) constrainedby
    Modelica.Mechanics.Translational.Components.Spring
    annotation(choicesAllMatching=true, Placement(transformation(origin={15,-15}, extent={{-15,-15},{15,15}})));

  //Connectors
  Modelica.Mechanics.Translational.Interfaces.Flange_a DamperPort annotation(Placement(transformation(origin={98,-28}, extent={{-20,-20},{20,20}})));
  Modelica.Mechanics.Translational.Interfaces.Flange_a MassPort annotation(Placement(transformation(origin={252,-99}, extent={{-20,-20},{20,20}})));
  Modelica.Mechanics.Translational.Interfaces.Flange_a SpringPort annotation(Placement(transformation(origin={99,-169}, extent={{-20,-20},{20,20}})));
equation
  connect(Damper_1__Damper_1.flange_a, DamperPort) annotation(Line(points = {{0.0,0.0},{0.1,0.1}}));
  connect(Damper_1__Damper_1.flange_b, Mass_1__Mass_5.flange_b) annotation(Line(points = {{0.0,0.0},{0.1,0.1}}));
  connect(Mass_1__Mass_5.flange_a, MassPort) annotation(Line(points = {{0.0,0.0},{0.1,0.1}}));
  connect(Mass_1__Mass_5.flange_b, Spring_1__Spring_1.flange_b) annotation(Line(points = {{0.0,0.0},{0.1,0.1}}));
  connect(Spring_1__Spring_1.flange_a, SpringPort) annotation(Line(points = {{0.0,0.0},{0.1,0.1}}));

 // Annotations
annotation (Icon(coordinateSystem(preserveAspectRatio=true,  extent={{0,-169},{252,0}}),
graphics={
Rectangle(extent={{0,0},{252,-169}},
lineColor={0,0,0},
fillColor={250,250,255},
fillPattern=FillPattern.Solid),
Polygon(points={{-40,-40},{-40,0},{0,0},{-40,-40}},
smooth=Smooth.None,
fillColor={0,127,0},
fillPattern=FillPattern.Solid,
pattern=LinePattern.None,
origin={0,40},
rotation=0,
lineColor={0,127,0}),
Polygon(points={{-40,-40},{-40,0},{0,0},{-40,-40}},
smooth=Smooth.None,
fillColor={0,127,0},
fillPattern=FillPattern.Solid,
pattern=LinePattern.None,
origin={252,-209},
rotation=180,
lineColor={0,127,0}),
Rectangle(
extent={{-40,50},{292,90}},
lineColor={0,127,0},
fillColor={0,127,0},
fillPattern=FillPattern.Solid),
Text(
extent={{-40,50},{292,90}},
lineColor={255,255,255},
textString="%name",
fontName="Comic Sans MS",
textStyle={TextStyle.Bold})}),
Diagram(coordinateSystem(
preserveAspectRatio=true,  extent={{0,-169},{252,0}})));
end MassSpringDamper;
