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

import io.netty.channel.ChannelHandlerContext;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

import java.util.ArrayList;
import java.util.HashSet;
import java.util.List;
import java.util.Set;
import java.util.concurrent.atomic.AtomicReference;

/**
 * When INTEREST is expressed these items are produced.
 *
 * User: feisele
 * Date: 8/7/13
 * Time: 9:05 AM
 * To change this template use File | Settings | File Templates.
 */
public class Interest implements Comparable<Interest> {
    private static final boolean REPLICATE_RACE = false;

    private static final Logger logger = LoggerFactory
            .getLogger("meta.link.msg.interest");

    private final List<String> topic;

    public Interest(final List<String> topic) {
        this.topic = topic;
    }

    public List<String> getTopic() {
        return this.topic;
    }

    @Override
    public int compareTo(Interest thatInterest) {
        return this.compareTopic(thatInterest.topic);
    }

    @Override
    public boolean equals(Object obj) {
        if (!(obj instanceof Interest)) {
            return false;
        }
        final Interest thatInterest = (Interest) obj;
        return (0 == this.compareTo(thatInterest));
    }

    public int compareTopic(List<String> thatTopic) {
        final int smaller = (this.topic.size() < thatTopic.size()) ? this.topic.size() : thatTopic.size();

        for (int ix=0; ix < smaller; ++ix) {
            final int compareTo = this.topic.get(ix).compareTo(thatTopic.get(ix));
            if (compareTo == 0) continue;
            return compareTo;
        }
        if (this.topic.size() < thatTopic.size()) return -1;
        if (this.topic.size() > thatTopic.size()) return 1;
        return 0;
    }

    @Override
    public String toString() {
        final StringBuilder builder = new StringBuilder();
        builder.append(" topic: ").append('[');
        for (final String item : this.topic) {
            builder.append("\n   ").append(item);
        }
        builder.append(']').append('\n');
        return builder.toString();
    }

    /**
     * A set of channels for a common interest.
     */
    static public class InterestedChannelSet {
        private final Interest interest;
        private final Set<ChannelHandlerContext> contextSet;

        public InterestedChannelSet(final Interest interest) {
            this.interest = interest;
            this.contextSet = new HashSet<ChannelHandlerContext>();
        }

        public Set<ChannelHandlerContext> getContextSet() {
            return this.contextSet;
        }
        
        public int size()
        {
            return this.contextSet.size();
        }

        synchronized public void addItem(final ChannelHandlerContext ctx, final Interest interest) {
            if (REPLICATE_RACE)
                try {
                    Thread.sleep(1000);
                } catch (InterruptedException ex) {
                    logger.debug("add to interested channel set");
                }
            if (REPLICATE_RACE)
                try {
                    Thread.sleep(1000);
                } catch (InterruptedException ex) {
                    logger.debug("add to interested channel set");
                }
            logger.debug("adding a context [{}] to interest [{}]",
                    ctx.channel().remoteAddress(), interest);
            if (! this.interest.equals(interest)) {
                logger.warn("adding mismatched interest [{}]", this.interest);
            }
            this.contextSet.add(ctx);
        }

        synchronized public void removeItem(final ChannelHandlerContext ctx, final Interest interest) {
            if (REPLICATE_RACE)
                try {
                    Thread.sleep(1000);
                } catch (InterruptedException ex) {
                    logger.debug("remove from interested channel set");
                }
            if (REPLICATE_RACE)
                try {
                    Thread.sleep(1000);
                } catch (InterruptedException ex) {
                    logger.debug("remove from interested channel set");
                }
            logger.debug("removing a context [{}] from interest[{}]",
                    ctx.channel().remoteAddress(), interest);

            if (! this.interest.equals(interest)) {
                logger.warn("remove mismatched interest[{}]", this.interest);
            }
            this.contextSet.remove(ctx);
        }
    }

    /**
     * The interests of a particular channel.
     */
    static public class ChannelInterestSet {
        public final ChannelHandlerContext context;
        private String origin;
        private final Set<Interest>  interestList;

        public ChannelInterestSet(final ChannelHandlerContext context) {
            this.context = context;
            this.interestList = new HashSet<Interest>();
        }

        public Set<Interest> getInterestList() {
            return this.interestList;
        }

        synchronized public void addItem(final ChannelHandlerContext ctx, final Interest interest) {
            if (REPLICATE_RACE)
                try {
                    Thread.sleep(1000);
                } catch (InterruptedException ex) {
                    logger.debug("add to channel interest set");
                }
            if (REPLICATE_RACE)
                try {
                    Thread.sleep(1000);
                } catch (InterruptedException ex) {
                    logger.debug("add to channel interest set");
                }
            /*
            if (! this.origin.equals(origin)) {

                logger.warn("adding an interest [{}] to mismatched origin [{}] : [{}]",
                        interest, origin, this.origin);
                this.origin = origin;
            }
            */
            if (ctx != this.context) {
                logger.warn("adding an interest [{}] to mismatched channel [{}] : [{}]",
                        interest, ctx, this.context);
            }
            this.interestList.add( interest );
        }

        synchronized public String removeItem(final ChannelHandlerContext ctx, final Interest interest) {
            if (REPLICATE_RACE)
                try {
                    Thread.sleep(1000);
                } catch (InterruptedException ex) {
                    logger.debug("remove from channel interest set");
                }
            if (REPLICATE_RACE)
                try {
                    Thread.sleep(1000);
                } catch (InterruptedException ex) {
                    logger.debug("remove from channel interest set");
                }
            if (ctx != this.context) {
                logger.warn("removing an interest [{}] to mismatched channel [{}] : [{}]",
                        interest, ctx, this.context);
            }
            this.interestList.remove(interest);
            return this.origin;
        }
    }

    @Override
    public  int hashCode() {
        final int prime = 31;
        int result = 1;
        for (final String item : this.topic) {
            result = result * prime + item.hashCode();
        }
        return result;
    }


}
