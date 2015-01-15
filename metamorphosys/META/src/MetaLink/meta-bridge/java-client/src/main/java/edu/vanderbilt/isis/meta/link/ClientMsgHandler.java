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

import edu.vanderbilt.isis.meta.MetaLinkMsg;
import io.netty.channel.ChannelHandlerContext;
import io.netty.channel.SimpleChannelInboundHandler;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

import java.io.File;
import java.util.concurrent.BlockingQueue;


// @Sharable
public class ClientMsgHandler extends SimpleChannelInboundHandler<MetaLinkMsg.Edit> {
    private static final Logger logger = LoggerFactory
            .getLogger(ClientMsgHandler.class);
    final BlockingQueue<MetaLinkMsg.Edit> messageQueue;
    private final SyntheticData.Recorder recorder;

    private ChannelHandlerContext context;

    /**
     *
     * @param messageQueue
     */
    public ClientMsgHandler(final BlockingQueue<MetaLinkMsg.Edit> messageQueue, final File recordFile) {
        this.messageQueue = messageQueue;

        Set_Recorder: {
            if (recordFile == null)  {
                logger.info("no recorder file specified");
                this.recorder = null;
                break Set_Recorder;
            }
            if (recordFile.exists())  {
                logger.warn("the recorder file is being overwritten", recordFile);
                if (! recordFile.canWrite())  {
                    logger.warn("the recorder file path provided is not writable: {}", recordFile);
                    this.recorder = null;
                    break Set_Recorder;
                }
            }
            this.recorder = new SyntheticData.Recorder(recordFile);
        }
    }

    @Override
    public void channelRead0(ChannelHandlerContext context, MetaLinkMsg.Edit in) throws Exception {
        logger.info("message received {}\n{}", Integer.toHexString(in.hashCode()), in);
        if (this.recorder != null) {
            this.recorder.record(context, in);
        }
        for (final MetaLinkMsg.Action action : in.getActionsList()) {
            if (action.hasAlien()) {
                 final MetaLinkMsg.Alien alien = action.getAlien();

                switch (alien.getEncodingMode()) {
                    case JSON: {
                        logger.info("json \n{}", alien.getEncoded());
                        break;
                    }
                    case XML:   {
                        logger.info("xml \n{}", alien.getEncoded());
                        break;
                    }
                    default:
                        logger.error("unknown encoding type {}", alien.getEncodingMode());
                }
            }
        }
    }

    /**
     * This method is called when there is a problem.
     * The Throwable is logged and the Channel is closed, which means the connection to the server is closed.
     *
     * @param ctx
     * @param cause
     */
    @Override
    public void exceptionCaught(ChannelHandlerContext ctx, Throwable cause) {
        logger.warn("exception caught {}", cause.getLocalizedMessage());
        ctx.close();
        if (this.recorder != null) {
            this.recorder.stopRecording();
        }
    }

    /**
     * Called once the connection to the Server is established.
     * Register with the distributor.
     * This method will be called once the connection is established.
     * Once the connection is established, a sequence of bytes is promptly sent to the Server.
     * Overriding this method ensures that something is written to the server as soon as possible.
     */
    @Override
    public void channelActive(ChannelHandlerContext ctx) throws Exception {
        super.channelActive(ctx);
        this.context = ctx;
        sendPending();
    }

    public void sendPending() throws InterruptedException  {
        logger.info("generating sample messages\n{}", this.messageQueue.size());
        final ChannelHandlerContext ctx = this.context;
        if (ctx == null) {
            logger.error("no active context");
            return;
        }
        for(int ix=0; ! this.messageQueue.isEmpty(); ++ix ) {
            final MetaLinkMsg.Edit msg = this.messageQueue.take();

            if (msg == null) {
                logger.error("null message");
                continue;
            }
            logger.info("sample message [{}]...\n{}", ix, msg);
            ctx.write(msg);
            ctx.flush();
        }
    }

    /**
     * Called once the connection to the server is dropped.
     * Unregister with the distributor.
     */
    @Override
    public void channelInactive(ChannelHandlerContext ctx) throws Exception {
        super.channelInactive(ctx);
        logger.warn("channel inactive {}", ctx);
        if (this.recorder != null) {
            this.recorder.stopRecording();
        }
    }

}