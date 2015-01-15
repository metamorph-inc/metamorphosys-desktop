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

package edu.vanderbilt.isis.meta.link;

import com.google.protobuf.ByteString;
import com.google.protobuf.TextFormat;
import edu.vanderbilt.isis.meta.AssemblyInterface;
import edu.vanderbilt.isis.meta.MetaLinkMsg;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

import java.security.SecureRandom;
import java.util.UUID;

/**
 * User: Fred Eisele
 * Date: 6/4/13
 * Time: 4:20 PM
 */
public enum SampleEditMsg {
    INSTANCE;
    private static final Logger logger = LoggerFactory
            .getLogger(SampleEditMsg.class);

    final MetaLinkMsg.Edit message;
    private SampleEditMsg() {

        final AssemblyInterface.CADParameterType cadParameter_A =
            AssemblyInterface.CADParameterType.newBuilder()
                .setUnits(AssemblyInterface.UnitsType.newBuilder().setValue("inch").build())
                .setName("diameter")
                .setType("tube")
                .setValue("12")
                .build();

        final AssemblyInterface.CADParameterType cadParameter_B =
            AssemblyInterface.CADParameterType.newBuilder()
                .setUnits(AssemblyInterface.UnitsType.newBuilder().setValue("inch").build())
                .setName("thickness")
                .setType("tube-wall")
                .setValue("0.1")
                .build();

        final AssemblyInterface.ParametricParametersType parameter_A =
                AssemblyInterface.ParametricParametersType.newBuilder()
                .addCADParameter(cadParameter_A)
                .addCADParameter(cadParameter_B)
                .build();

        final AssemblyInterface.CADComponentType cadComponent_A =
                AssemblyInterface.CADComponentType.newBuilder()
                        .setComponentID(UUID.randomUUID().toString())
                        .setName("Fuel Pump " + new SecureRandom().nextInt())
                        .setParametricParameters(parameter_A)
                        .build();

        final MetaLinkMsg.Payload payload_A =
                MetaLinkMsg.Payload.newBuilder()
                        .addComponents(cadComponent_A)
                        .build();

        final MetaLinkMsg.Action action_A =
                MetaLinkMsg.Action.newBuilder()
                        .setActionMode(MetaLinkMsg.Action.ActionMode.INSERT)
                        .setPayload(payload_A)
                        .build();

        final AssemblyInterface.CADComponentType cadComponent_B =
                AssemblyInterface.CADComponentType.newBuilder()
                        .setComponentID(UUID.randomUUID().toString())
                        .setName("Fuel Tank " + new SecureRandom().nextInt())
                        .build();

        final MetaLinkMsg.Payload payload_B =
                MetaLinkMsg.Payload.newBuilder()
                        .addComponents(cadComponent_B)
                        .build();

        final MetaLinkMsg.Environment environment_B =
                MetaLinkMsg.Environment.newBuilder()
                        .setName("FOO")
                        .addValue("bar")
                        .build();

        final ByteString quoted_string =
                ByteString.copyFromUtf8(" some bytes which aren't really xml but does contain both kinds of quote \" ");
        final MetaLinkMsg.Alien alien_B =
                MetaLinkMsg.Alien.newBuilder()
                .setEncodingMode(MetaLinkMsg.Alien.EncodingMode.XML)
                .setEncoded(quoted_string)
                .build();

        final MetaLinkMsg.Action action_B =
                MetaLinkMsg.Action.newBuilder()
                        .setActionMode(MetaLinkMsg.Action.ActionMode.INSERT)
                        .setPayload(payload_B)
                        .addEnvironment(environment_B)
                        .setAlien(alien_B)
                        .build();

        this.message =
                MetaLinkMsg.Edit.newBuilder()
                        .setEditMode(MetaLinkMsg.Edit.EditMode.POST)
                        .addTopic("ISIS.METALINK.CADASSEMBLY")
                        .addTopic("default assembly id")
                        .addOrigin("sample-value")
                        .addOrigin("sample-value")
                        .setGuid(UUID.randomUUID().toString())
                        .addActions(action_A)
                        .addActions(action_B)
                        .build();
    }


    public String asString() {
        return TextFormat.printToString(this.message);
    }


    public MetaLinkMsg.Edit asMessage() {
        return this.message;
    }
}
