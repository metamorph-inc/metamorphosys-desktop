<?xml version="1.0"?>
<eagle xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" version="6.5.0" xmlns="eagle">
  <compatibility />
  <drawing>
    <settings>
      <setting alwaysvectorfont="no" />
      <setting />
    </settings>
    <grid distance="0.01" unitdist="inch" unit="inch" display="yes" altdistance="0.01" altunitdist="inch" altunit="inch" />
    <layers>
      <layer number="1" name="Top" color="4" fill="1" visible="no" active="no" />
      <layer number="16" name="Bottom" color="1" fill="1" visible="no" active="no" />
      <layer number="17" name="Pads" color="2" fill="1" visible="no" active="no" />
      <layer number="18" name="Vias" color="2" fill="1" visible="no" active="no" />
      <layer number="19" name="Unrouted" color="6" fill="1" visible="no" active="no" />
      <layer number="20" name="Dimension" color="15" fill="1" visible="no" active="no" />
      <layer number="21" name="tPlace" color="7" fill="1" visible="no" active="no" />
      <layer number="22" name="bPlace" color="7" fill="1" visible="no" active="no" />
      <layer number="23" name="tOrigins" color="15" fill="1" visible="no" active="no" />
      <layer number="24" name="bOrigins" color="15" fill="1" visible="no" active="no" />
      <layer number="25" name="tNames" color="7" fill="1" visible="no" active="no" />
      <layer number="26" name="bNames" color="7" fill="1" visible="no" active="no" />
      <layer number="27" name="tValues" color="7" fill="1" visible="no" active="no" />
      <layer number="28" name="bValues" color="7" fill="1" visible="no" active="no" />
      <layer number="29" name="tStop" color="7" fill="3" visible="no" active="no" />
      <layer number="30" name="bStop" color="7" fill="6" visible="no" active="no" />
      <layer number="31" name="tCream" color="7" fill="4" visible="no" active="no" />
      <layer number="32" name="bCream" color="7" fill="5" visible="no" active="no" />
      <layer number="33" name="tFinish" color="6" fill="3" visible="no" active="no" />
      <layer number="34" name="bFinish" color="6" fill="6" visible="no" active="no" />
      <layer number="35" name="tGlue" color="7" fill="4" visible="no" active="no" />
      <layer number="36" name="bGlue" color="7" fill="5" visible="no" active="no" />
      <layer number="37" name="tTest" color="7" fill="1" visible="no" active="no" />
      <layer number="38" name="bTest" color="7" fill="1" visible="no" active="no" />
      <layer number="39" name="tKeepout" color="4" fill="11" visible="no" active="no" />
      <layer number="40" name="bKeepout" color="1" fill="11" visible="no" active="no" />
      <layer number="41" name="tRestrict" color="4" fill="10" visible="no" active="no" />
      <layer number="42" name="bRestrict" color="1" fill="10" visible="no" active="no" />
      <layer number="43" name="vRestrict" color="2" fill="10" visible="no" active="no" />
      <layer number="44" name="Drills" color="7" fill="1" visible="no" active="no" />
      <layer number="45" name="Holes" color="7" fill="1" visible="no" active="no" />
      <layer number="46" name="Milling" color="3" fill="1" visible="no" active="no" />
      <layer number="47" name="Measures" color="7" fill="1" visible="no" active="no" />
      <layer number="48" name="Document" color="7" fill="1" visible="no" active="no" />
      <layer number="49" name="Reference" color="7" fill="1" visible="no" active="no" />
      <layer number="51" name="tDocu" color="7" fill="1" visible="no" active="no" />
      <layer number="52" name="bDocu" color="7" fill="1" visible="no" active="no" />
      <layer number="91" name="Nets" color="2" fill="1" />
      <layer number="92" name="Busses" color="1" fill="1" />
      <layer number="93" name="Pins" color="2" fill="1" visible="no" />
      <layer number="94" name="Symbols" color="4" fill="1" />
      <layer number="95" name="Names" color="7" fill="1" />
      <layer number="96" name="Values" color="7" fill="1" />
      <layer number="97" name="Info" color="7" fill="1" />
      <layer number="98" name="Guide" color="6" fill="1" />
    </layers>
    <schematic xrefpart="/%S.%C%R" xreflabel="%F%N/%S.%C%R">
      <description />
      <libraries>
        <library name="mlcc">
          <description />
          <packages>
            <package name="C_0805">
              <description>&lt;B&gt; 0805&lt;/B&gt; (2012 Metric) MLCC Capacitor &lt;P&gt;</description>
              <wire x1="-1" y1="0.625" x2="1" y2="0.625" width="0.0762" layer="51" />
              <wire x1="1" y1="0.625" x2="1" y2="-0.625" width="0.0762" layer="51" />
              <wire x1="1" y1="-0.625" x2="-1" y2="-0.625" width="0.0762" layer="51" />
              <wire x1="-1" y1="-0.625" x2="-1" y2="0.625" width="0.0762" layer="51" />
              <wire x1="-0.1016" y1="0.7112" x2="0.1016" y2="0.7112" width="0.1524" layer="21" />
              <wire x1="-0.1016" y1="-0.7112" x2="0.1016" y2="-0.7112" width="0.1524" layer="21" />
              <smd name="1" x="-1" y="0" dx="1.35" dy="1.55" layer="1" />
              <smd name="2" x="1" y="0" dx="1.35" dy="1.55" layer="1" />
              <text x="-1.8" y="1" size="1.016" layer="25" font="vector" ratio="15">&gt;NAME</text>
            </package>
          </packages>
          <symbols>
            <symbol name="CAP_NP">
              <description>&lt;B&gt;Capacitor&lt;/B&gt; -- non-polarized</description>
              <wire x1="-1.905" y1="-3.175" x2="0" y2="-3.175" width="0.6096" layer="94" />
              <wire x1="0" y1="-3.175" x2="1.905" y2="-3.175" width="0.6096" layer="94" />
              <wire x1="-1.905" y1="-4.445" x2="0" y2="-4.445" width="0.6096" layer="94" />
              <wire x1="0" y1="-4.445" x2="1.905" y2="-4.445" width="0.6096" layer="94" />
              <wire x1="0" y1="-2.54" x2="0" y2="-3.175" width="0.254" layer="94" />
              <wire x1="0" y1="-5.08" x2="0" y2="-4.445" width="0.254" layer="94" />
              <pin name="P$1" x="0" y="0" visible="off" length="short" direction="pas" rot="R270" />
              <pin name="P$2" x="0" y="-7.62" visible="off" length="short" direction="pas" rot="R90" />
              <text x="-2.54" y="-7.62" size="1.778" layer="96" rot="R90">&gt;VALUE</text>
              <text x="-5.08" y="-7.62" size="1.778" layer="95" rot="R90">&gt;NAME</text>
              <text x="0.508" y="-2.286" size="1.778" layer="95">1</text>
            </symbol>
          </symbols>
          <devicesets>
            <deviceset prefix="C" name="C_0805">
              <description />
              <gates>
                <gate name="G$1" symbol="CAP_NP" x="0" y="0" />
              </gates>
              <devices>
                <device package="C_0805">
                  <connects>
                    <connect gate="G$1" pin="P$1" pad="1" />
                    <connect gate="G$1" pin="P$2" pad="2" />
                  </connects>
                  <technologies>
                    <technology name="" />
                  </technologies>
                </device>
              </devices>
            </deviceset>
          </devicesets>
        </library>
        <library name="ecad">
          <description />
          <packages>
            <package name="S-PDSO-G8">
              <description />
              <smd name="P$1" x="-2.05" y="0.975" dx="0.4" dy="1" layer="1" rot="R270" />
              <smd name="P$2" x="-2.05" y="0.325" dx="0.4" dy="1" layer="1" rot="R270" />
              <smd name="P$3" x="-2.05" y="-0.325" dx="0.4" dy="1" layer="1" rot="R270" />
              <smd name="P$4" x="-2.05" y="-0.975" dx="0.4" dy="1" layer="1" rot="R270" />
              <smd name="P$5" x="2.05" y="0.975" dx="0.4" dy="1" layer="1" rot="R270" />
              <smd name="P$6" x="2.05" y="0.325" dx="0.4" dy="1" layer="1" rot="R270" />
              <smd name="P$7" x="2.05" y="-0.325" dx="0.4" dy="1" layer="1" rot="R270" />
              <smd name="P$8" x="2.05" y="-0.975" dx="0.4" dy="1" layer="1" rot="R270" />
              <circle x="-2.675" y="1.7" radius="0.15239999999999998" width="0" layer="21" />
              <text x="-3.3" y="1.97" size="1.016" layer="25" font="vector" ratio="15">&gt;NAME</text>
              <wire x1="1.35" y1="1.55" x2="-1.35" y2="1.55" width="0.15239999999999998" layer="21" />
              <wire x1="1.35" y1="-1.55" x2="-1.35" y2="-1.55" width="0.15239999999999998" layer="21" />
            </package>
            <package name="0603-LED-KINGBRIGHT">
              <description />
              <smd name="1" x="-0.825" y="0" dx="0.8" dy="0.8" layer="1" />
              <smd name="2" x="0.825" y="0" dx="0.8" dy="0.8" layer="1" />
              <wire x1="-1.7" y1="-0.85" x2="-1.7" y2="0.85" width="0.1542" layer="21" />
              <wire x1="-1.7" y1="0.85" x2="1.7" y2="0.85" width="0.1542" layer="21" />
              <wire x1="1.7" y1="0.85" x2="1.7" y2="-0.85" width="0.1542" layer="21" />
              <wire x1="1.7" y1="-0.85" x2="-1.7" y2="-0.85" width="0.1542" layer="21" />
              <polygon layer="21" width="0.127">
                <vertex x="-1.7" y="-0.425" />
                <vertex x="-1.275" y="-0.85" />
                <vertex x="-1.7" y="-0.85" />
              </polygon>
              <circle x="-1.275" y="-1.275" radius="0.1542" width="0" layer="21" />
              <text x="-2.55" y="1.275" size="1.016" layer="25" font="vector" ratio="15">&gt;NAME</text>
            </package>
          </packages>
          <symbols>
            <symbol name="LM555D">
              <description />
              <pin name="GND" x="-22.86" y="7.62" length="middle" direction="pas" />
              <pin name="TRIGGER" x="-22.86" y="2.54" length="middle" direction="pas" />
              <pin name="OUTPUT" x="-22.86" y="-2.54" length="middle" direction="pas" />
              <pin name="RESET" x="-22.86" y="-7.62" length="middle" direction="pas" />
              <pin name="VCC" x="22.86" y="7.62" length="middle" direction="pas" rot="R180" />
              <pin name="DISCHARGE" x="22.86" y="2.54" length="middle" direction="pas" rot="R180" />
              <pin name="THRESHOLD" x="22.86" y="-2.54" length="middle" direction="pas" rot="R180" />
              <pin name="CONTROL_VOLTAGE" x="22.86" y="-7.62" length="middle" direction="pas" rot="R180" />
              <wire x1="-17.78" y1="10.16" x2="-17.78" y2="-10.16" width="0.254" layer="94" />
              <wire x1="-17.78" y1="-10.16" x2="17.78" y2="-10.16" width="0.254" layer="94" />
              <wire x1="17.78" y1="-10.16" x2="17.78" y2="10.16" width="0.254" layer="94" />
              <wire x1="17.78" y1="10.16" x2="-17.78" y2="10.16" width="0.254" layer="94" />
              <text x="-17.78" y="15.24" size="1.778" layer="95">&gt;NAME</text>
              <text x="-17.78" y="12.7" size="1.778" layer="96">&gt;VALUE</text>
            </symbol>
            <symbol name="LED_WITH_PIN1_CATHODE">
              <description>&lt;B&gt;LED with pin 1 cathode&lt;/B&gt;&lt;P&gt;
Designed for the Osram LB Q39G-L2N2-35-1
blue led, in an 0603 package.&lt;P&gt;
Digikey 475-2816-1-ND</description>
              <wire x1="-1.27" y1="2.54" x2="1.27" y2="2.54" width="0.254" layer="94" />
              <wire x1="1.778" y1="5.08" x2="2.54" y2="5.842" width="0.254" layer="94" />
              <wire x1="2.54" y1="5.842" x2="2.54" y2="5.334" width="0.254" layer="94" />
              <wire x1="2.54" y1="5.334" x2="3.048" y2="5.842" width="0.254" layer="94" />
              <wire x1="1.524" y1="3.81" x2="2.286" y2="4.572" width="0.254" layer="94" />
              <wire x1="2.286" y1="4.572" x2="2.286" y2="4.064" width="0.254" layer="94" />
              <wire x1="2.286" y1="4.064" x2="2.794" y2="4.572" width="0.254" layer="94" />
              <pin name="C" x="0" y="0" visible="off" length="short" direction="pas" rot="R90" />
              <pin name="A" x="0" y="7.62" visible="off" length="short" direction="pas" rot="R270" />
              <text x="-5.08" y="0" size="1.778" layer="95" rot="R90">&gt;NAME</text>
              <text x="-2.54" y="0" size="1.778" layer="95" rot="R90">&gt;VALUE</text>
              <polygon layer="94" width="0.254">
                <vertex x="0" y="2.54" />
                <vertex x="-1.27" y="5.08" />
                <vertex x="1.27" y="5.08" />
              </polygon>
              <polygon layer="94" width="0.254">
                <vertex x="3.556" y="6.35" />
                <vertex x="3.048" y="6.096" />
                <vertex x="3.302" y="5.842" />
              </polygon>
              <polygon layer="94" width="0.254">
                <vertex x="3.302" y="5.08" />
                <vertex x="2.794" y="4.826" />
                <vertex x="3.048" y="4.572" />
              </polygon>
            </symbol>
          </symbols>
          <devicesets>
            <deviceset name="LM555CMM/NOPB">
              <description />
              <gates>
                <gate name="G$1" symbol="LM555D" x="0" y="0" />
              </gates>
              <devices>
                <device package="S-PDSO-G8">
                  <connects>
                    <connect gate="G$1" pin="CONTROL_VOLTAGE" pad="P$5" />
                    <connect gate="G$1" pin="DISCHARGE" pad="P$7" />
                    <connect gate="G$1" pin="GND" pad="P$1" />
                    <connect gate="G$1" pin="OUTPUT" pad="P$3" />
                    <connect gate="G$1" pin="RESET" pad="P$4" />
                    <connect gate="G$1" pin="THRESHOLD" pad="P$6" />
                    <connect gate="G$1" pin="TRIGGER" pad="P$2" />
                    <connect gate="G$1" pin="VCC" pad="P$8" />
                  </connects>
                  <technologies>
                    <technology name="" />
                  </technologies>
                </device>
              </devices>
            </deviceset>
            <deviceset name="LED-0603-KINGBRIGHT">
              <description />
              <gates>
                <gate name="G$1" symbol="LED_WITH_PIN1_CATHODE" x="0" y="0" />
              </gates>
              <devices>
                <device package="0603-LED-KINGBRIGHT">
                  <connects>
                    <connect gate="G$1" pin="A" pad="2" />
                    <connect gate="G$1" pin="C" pad="1" />
                  </connects>
                  <technologies>
                    <technology name="" />
                  </technologies>
                </device>
              </devices>
            </deviceset>
          </devicesets>
        </library>
        <library name="resistor">
          <description />
          <packages>
            <package name="R_0603">
              <description>&lt;B&gt;
0603
&lt;/B&gt; SMT inch-code chip resistor package&lt;P&gt;
Derived from dimensions and tolerances
in the &lt;B&gt; &lt;A HREF="http://www.samsungsem.com/global/support/library/product-catalog/__icsFiles/afieldfile/2015/01/12/CHIP_RESISTOR_150112_1.pdf"&gt;Samsung Thick Film Chip Resistor Catalog&lt;/A&gt;&lt;/B&gt;
dated December 2014,
for 
general-purpose chip resistor
reflow soldering.</description>
              <wire x1="-0.1" y1="0.3" x2="0.1" y2="0.3" width="0.1524" layer="21" />
              <wire x1="-0.1" y1="-0.3" x2="0.1" y2="-0.3" width="0.1524" layer="21" />
              <smd name="P$1" x="-0.8" y="0" dx="0.8" dy="0.8" layer="1" />
              <smd name="P$2" x="0.8" y="0" dx="0.8" dy="0.8" layer="1" />
              <text x="-1.3" y="0.8" size="1.016" layer="25" font="vector" ratio="15">&gt;NAME</text>
              <wire x1="-0.8" y1="0.4" x2="0.8" y2="0.4" width="0.0762" layer="51" />
              <wire x1="0.8" y1="0.4" x2="0.8" y2="-0.4" width="0.0762" layer="51" />
              <wire x1="0.8" y1="-0.4" x2="-0.8" y2="-0.4" width="0.0762" layer="51" />
              <wire x1="-0.8" y1="-0.4" x2="-0.8" y2="0.4" width="0.0762" layer="51" />
            </package>
          </packages>
          <symbols>
            <symbol name="R">
              <description>&lt;B&gt;Resistor&lt;/B&gt;</description>
              <wire x1="-2.54" y1="0" x2="-2.159" y2="1.016" width="0.2032" layer="94" />
              <wire x1="-2.159" y1="1.016" x2="-1.524" y2="-1.016" width="0.2032" layer="94" />
              <wire x1="-1.524" y1="-1.016" x2="-0.889" y2="1.016" width="0.2032" layer="94" />
              <wire x1="-0.889" y1="1.016" x2="-0.254" y2="-1.016" width="0.2032" layer="94" />
              <wire x1="-0.254" y1="-1.016" x2="0.381" y2="1.016" width="0.2032" layer="94" />
              <wire x1="0.381" y1="1.016" x2="1.016" y2="-1.016" width="0.2032" layer="94" />
              <wire x1="1.016" y1="-1.016" x2="1.651" y2="1.016" width="0.2032" layer="94" />
              <wire x1="1.651" y1="1.016" x2="2.286" y2="-1.016" width="0.2032" layer="94" />
              <wire x1="2.286" y1="-1.016" x2="2.54" y2="0" width="0.2032" layer="94" />
              <pin name="1" x="-5.08" y="0" visible="off" length="short" direction="pas" swaplevel="1" />
              <pin name="2" x="5.08" y="0" visible="off" length="short" direction="pas" swaplevel="1" rot="R180" />
              <text x="-3.81" y="1.4986" size="1.778" layer="95">&gt;NAME</text>
              <text x="-3.81" y="-3.302" size="1.778" layer="96">&gt;VALUE</text>
            </symbol>
          </symbols>
          <devicesets>
            <deviceset prefix="R" name="RESISTOR_0603">
              <description />
              <gates>
                <gate name="G$1" symbol="R" x="0" y="0" />
              </gates>
              <devices>
                <device package="R_0603">
                  <connects>
                    <connect gate="G$1" pin="1" pad="P$1" />
                    <connect gate="G$1" pin="2" pad="P$2" />
                  </connects>
                  <technologies>
                    <technology name="" />
                  </technologies>
                </device>
              </devices>
            </deviceset>
          </devicesets>
        </library>
        <library name="General_Will">
          <description />
          <packages>
            <package name="1X2VIA">
              <description />
              <pad name="P$1" x="0" y="0" drill="0.8" />
              <pad name="P$2" x="0" y="2.54" drill="0.8" />
              <wire x1="-0.635" y1="3.81" x2="-1.27" y2="3.175" width="0.127" layer="21" />
              <wire x1="-1.27" y1="3.175" x2="-1.27" y2="1.905" width="0.127" layer="21" />
              <wire x1="-1.27" y1="1.905" x2="-0.635" y2="1.27" width="0.127" layer="21" />
              <wire x1="-0.635" y1="1.27" x2="-1.27" y2="0.635" width="0.127" layer="21" />
              <wire x1="-1.27" y1="0.635" x2="-1.27" y2="-0.635" width="0.127" layer="21" />
              <wire x1="-1.27" y1="-0.635" x2="-0.635" y2="-1.27" width="0.127" layer="21" />
              <wire x1="-0.635" y1="-1.27" x2="0.635" y2="-1.27" width="0.127" layer="21" />
              <wire x1="0.635" y1="-1.27" x2="1.27" y2="-0.635" width="0.127" layer="21" />
              <wire x1="1.27" y1="-0.635" x2="1.27" y2="0.635" width="0.127" layer="21" />
              <wire x1="1.27" y1="0.635" x2="0.635" y2="1.27" width="0.127" layer="21" />
              <wire x1="0.635" y1="1.27" x2="1.27" y2="1.905" width="0.127" layer="21" />
              <wire x1="1.27" y1="1.905" x2="1.27" y2="3.175" width="0.127" layer="21" />
              <wire x1="1.27" y1="3.175" x2="0.635" y2="3.81" width="0.127" layer="21" />
              <wire x1="0.635" y1="3.81" x2="-0.635" y2="3.81" width="0.127" layer="21" />
              <circle x="-1.905" y="0" radius="0.1542" width="0" layer="21" />
              <text x="-3.175" y="4.445" size="1.27" layer="25">&gt;NAME</text>
              <text x="-3.175" y="-0.635" size="1.27" layer="25">+</text>
            </package>
          </packages>
          <symbols>
            <symbol name="BATTERY_CONNECT">
              <description />
              <pin name="POS" x="-2.54" y="5.08" length="short" rot="R270" />
              <pin name="NEG" x="2.54" y="5.08" length="short" rot="R270" />
              <wire x1="-7.62" y1="2.54" x2="-7.62" y2="-7.62" width="0.254" layer="94" />
              <wire x1="-7.62" y1="-7.62" x2="7.62" y2="-7.62" width="0.254" layer="94" />
              <wire x1="7.62" y1="-7.62" x2="7.62" y2="2.54" width="0.254" layer="94" />
              <wire x1="7.62" y1="2.54" x2="-7.62" y2="2.54" width="0.254" layer="94" />
            </symbol>
          </symbols>
          <devicesets>
            <deviceset name="BS6I">
              <description />
              <gates>
                <gate name="G$1" symbol="BATTERY_CONNECT" x="0" y="0" />
              </gates>
              <devices>
                <device package="1X2VIA">
                  <connects>
                    <connect gate="G$1" pin="NEG" pad="P$2" />
                    <connect gate="G$1" pin="POS" pad="P$1" />
                  </connects>
                  <technologies>
                    <technology name="" />
                  </technologies>
                </device>
              </devices>
            </deviceset>
          </devicesets>
        </library>
        <library name="TL3342F160QG-697586">
          <description />
          <packages>
            <package name="TL3342F160QG">
              <description />
              <text x="-3" y="3" size="1.27" layer="25">&gt;Name</text>
              <text x="-3" y="-4" size="1.27" layer="27">&gt;VALUE</text>
              <smd name="3" x="-3.1" y="1.85" dx="1.8" dy="1.1" layer="1" />
              <smd name="4" x="3.1" y="1.85" dx="1.8" dy="1.1" layer="1" />
              <smd name="1" x="-3.1" y="-1.85" dx="1.8" dy="1.1" layer="1" />
              <smd name="2" x="3.1" y="-1.85" dx="1.8" dy="1.1" layer="1" />
            </package>
          </packages>
          <symbols>
            <symbol name="TL3342F160QG">
              <description />
              <circle x="-12.7" y="5.08" radius="2.54" width="0.254" layer="94" />
              <circle x="12.7" y="5.08" radius="2.54" width="0.254" layer="94" />
              <circle x="0" y="-5.08" radius="2.54" width="0.254" layer="94" />
              <circle x="0" y="-12.7" radius="2.54" width="0.254" layer="94" />
              <wire x1="-12.7" y1="2.54" x2="-12.7" y2="0" width="0.254" layer="94" />
              <wire x1="-12.7" y1="0" x2="12.7" y2="0" width="0.254" layer="94" />
              <wire x1="12.7" y1="0" x2="12.7" y2="2.54" width="0.254" layer="94" />
              <circle x="-12.7" y="-22.86" radius="2.54" width="0.254" layer="94" />
              <circle x="12.7" y="-22.86" radius="2.54" width="0.254" layer="94" />
              <wire x1="-12.7" y1="-20.32" x2="-12.7" y2="-17.78" width="0.254" layer="94" />
              <wire x1="-12.7" y1="-17.78" x2="12.7" y2="-17.78" width="0.254" layer="94" />
              <wire x1="12.7" y1="-17.78" x2="12.7" y2="-20.32" width="0.254" layer="94" />
              <wire x1="5.08" y1="-2.54" x2="5.08" y2="-8.89" width="0.254" layer="94" />
              <wire x1="5.08" y1="-8.89" x2="5.08" y2="-15.24" width="0.254" layer="94" />
              <wire x1="5.08" y1="-8.89" x2="13.97" y2="-8.89" width="0.254" layer="94" />
              <text x="-17.78" y="-35.56" size="1.778" layer="95">&gt;NAME</text>
              <text x="-17.78" y="-38.1" size="1.778" layer="96">&gt;VALUE</text>
              <pin name="3" x="-12.7" y="10.16" length="middle" direction="pas" rot="R270" />
              <pin name="4" x="12.7" y="10.16" length="middle" direction="pas" rot="R270" />
              <pin name="1" x="-12.7" y="-27.94" length="middle" direction="pas" rot="R90" />
              <pin name="2" x="12.7" y="-27.94" length="middle" direction="pas" rot="R90" />
            </symbol>
          </symbols>
          <devicesets>
            <deviceset prefix="S" name="TL3342F160QG">
              <description />
              <gates>
                <gate name="G$1" symbol="TL3342F160QG" x="0" y="7.62" />
              </gates>
              <devices>
                <device package="TL3342F160QG">
                  <connects>
                    <connect gate="G$1" pin="1" pad="1" />
                    <connect gate="G$1" pin="2" pad="2" />
                    <connect gate="G$1" pin="3" pad="3" />
                    <connect gate="G$1" pin="4" pad="4" />
                  </connects>
                  <technologies>
                    <technology name="" />
                  </technologies>
                </device>
              </devices>
            </deviceset>
          </devicesets>
        </library>
        <library name="SparkFun-Electromechanical">
          <description />
          <packages>
            <package name="BUZZER-12MM-KIT">
              <description />
              <circle x="0" y="0" radius="5.9" width="0.2032" layer="21" />
              <circle x="0" y="0" radius="1.27" width="0.2032" layer="51" />
              <pad name="-" x="-3.25" y="0" drill="0.9" diameter="1.8796" stop="no" />
              <pad name="+" x="3.25" y="0" drill="0.9" diameter="1.8796" stop="no" />
              <text x="-2.54" y="2.54" size="1.016" layer="25" font="vector" ratio="15">&gt;NAME</text>
              <text x="2.667" y="1.143" size="1.016" layer="51" font="vector" ratio="15">+</text>
              <polygon layer="30" width="0.127">
                <vertex x="3.2537" y="-0.9525" curve="-90" />
                <vertex x="2.2988" y="-0.0228" curve="-90.011749" />
                <vertex x="3.2512" y="0.9526" curve="-90" />
                <vertex x="4.2012" y="-0.0254" curve="-90.024193" />
              </polygon>
              <polygon layer="29" width="0.127">
                <vertex x="3.2512" y="-0.4445" curve="-90.012891" />
                <vertex x="2.8067" y="-0.0203" curve="-90" />
                <vertex x="3.2512" y="0.447" curve="-90" />
                <vertex x="3.6931" y="-0.0101" curve="-90.012967" />
              </polygon>
              <polygon layer="30" width="0.127">
                <vertex x="-3.2487" y="-0.9525" curve="-90" />
                <vertex x="-4.2036" y="-0.0228" curve="-90.011749" />
                <vertex x="-3.2512" y="0.9526" curve="-90" />
                <vertex x="-2.3012" y="-0.0254" curve="-90.024193" />
              </polygon>
              <polygon layer="29" width="0.127">
                <vertex x="-3.2512" y="-0.4445" curve="-90.012891" />
                <vertex x="-3.6957" y="-0.0203" curve="-90" />
                <vertex x="-3.2512" y="0.447" curve="-90" />
                <vertex x="-2.8093" y="-0.0101" curve="-90.012967" />
              </polygon>
            </package>
          </packages>
          <symbols>
            <symbol name="BUZZER">
              <description />
              <wire x1="-1.27" y1="1.905" x2="0" y2="1.905" width="0.1524" layer="94" />
              <wire x1="0" y1="1.905" x2="0" y2="2.54" width="0.1524" layer="94" />
              <wire x1="0" y1="1.905" x2="0" y2="1.27" width="0.1524" layer="94" />
              <wire x1="0.635" y1="3.175" x2="0.635" y2="0.635" width="0.1524" layer="94" />
              <wire x1="0.635" y1="0.635" x2="1.905" y2="0.635" width="0.1524" layer="94" />
              <wire x1="1.905" y1="0.635" x2="1.905" y2="3.175" width="0.1524" layer="94" />
              <wire x1="1.905" y1="3.175" x2="0.635" y2="3.175" width="0.1524" layer="94" />
              <wire x1="2.54" y1="2.54" x2="2.54" y2="1.905" width="0.1524" layer="94" />
              <wire x1="2.54" y1="1.905" x2="3.81" y2="1.905" width="0.1524" layer="94" />
              <wire x1="2.54" y1="1.905" x2="2.54" y2="1.27" width="0.1524" layer="94" />
              <wire x1="5.08" y1="0" x2="5.08" y2="3.81" width="0.254" layer="94" />
              <wire x1="5.08" y1="3.81" x2="5.715" y2="3.81" width="0.254" layer="94" />
              <wire x1="5.715" y1="3.81" x2="5.715" y2="4.445" width="0.254" layer="94" />
              <wire x1="5.715" y1="4.445" x2="-3.175" y2="4.445" width="0.254" layer="94" />
              <wire x1="-3.175" y1="4.445" x2="-3.175" y2="3.81" width="0.254" layer="94" />
              <wire x1="-3.175" y1="3.81" x2="-2.54" y2="3.81" width="0.254" layer="94" />
              <wire x1="-2.54" y1="3.81" x2="-2.54" y2="0" width="0.254" layer="94" />
              <wire x1="-2.54" y1="3.81" x2="5.08" y2="3.81" width="0.254" layer="94" />
              <wire x1="-2.54" y1="0" x2="5.08" y2="0" width="0.254" layer="94" />
              <text x="-2.54" y="5.08" size="1.778" layer="95">&gt;NAME</text>
              <text x="6.35" y="0" size="1.778" layer="96">&gt;VALUE</text>
              <pin name="2" x="2.54" y="-2.54" visible="off" length="short" direction="pas" rot="R90" />
              <pin name="1" x="0" y="-2.54" visible="off" length="short" direction="pas" rot="R90" />
            </symbol>
          </symbols>
          <devicesets>
            <deviceset prefix="SG" name="BUZZER">
              <description />
              <gates>
                <gate name="G$1" symbol="BUZZER" x="0" y="0" />
              </gates>
              <devices>
                <device package="BUZZER-12MM-KIT" name="PTH-KIT">
                  <connects>
                    <connect gate="G$1" pin="1" pad="+" />
                    <connect gate="G$1" pin="2" pad="-" />
                  </connects>
                  <technologies>
                    <technology name="">
                      <attribute name="PROD_ID" value="COMP-08253" />
                    </technology>
                  </technologies>
                </device>
              </devices>
            </deviceset>
          </devicesets>
        </library>
        <library name="magic_man">
          <description />
          <packages>
            <package name="BAT54">
              <description />
              <smd name="2" x="0.95" y="-1" dx="0.8" dy="0.9" layer="1" rot="R180" />
              <smd name="1" x="-0.95" y="-1" dx="0.8" dy="0.9" layer="1" rot="R180" />
              <smd name="3" x="0" y="1" dx="0.8" dy="0.9" layer="1" rot="R180" />
              <wire x1="1.7" y1="-1.8" x2="1.7" y2="1.8" width="0.127" layer="21" />
              <wire x1="1.7" y1="1.8" x2="-1.7" y2="1.8" width="0.127" layer="21" />
              <wire x1="-1.7" y1="1.8" x2="-1.7" y2="-1.8" width="0.127" layer="21" />
              <wire x1="-1.7" y1="-1.8" x2="1.7" y2="-1.8" width="0.127" layer="21" />
              <text x="-2" y="2.07" size="1" layer="21" font="vector" ratio="12">&gt;NAME</text>
              <circle x="-2.1" y="-1.3" radius="0.0635" width="0.16" layer="21" />
            </package>
          </packages>
          <symbols>
            <symbol name="LED_WITH_PIN1_CATHODE">
              <description>&lt;B&gt;LED with pin 1 cathode&lt;/B&gt;&lt;P&gt;
Designed for the Osram LB Q39G-L2N2-35-1
blue led, in an 0603 package.&lt;P&gt;
Digikey 475-2816-1-ND</description>
              <wire x1="-1.27" y1="2.54" x2="1.27" y2="2.54" width="0.254" layer="94" />
              <wire x1="1.778" y1="5.08" x2="2.54" y2="5.842" width="0.254" layer="94" />
              <wire x1="2.54" y1="5.842" x2="2.54" y2="5.334" width="0.254" layer="94" />
              <wire x1="2.54" y1="5.334" x2="3.048" y2="5.842" width="0.254" layer="94" />
              <wire x1="1.524" y1="3.81" x2="2.286" y2="4.572" width="0.254" layer="94" />
              <wire x1="2.286" y1="4.572" x2="2.286" y2="4.064" width="0.254" layer="94" />
              <wire x1="2.286" y1="4.064" x2="2.794" y2="4.572" width="0.254" layer="94" />
              <pin name="C" x="0" y="0" visible="off" length="short" direction="pas" rot="R90" />
              <pin name="A" x="0" y="7.62" visible="off" length="short" direction="pas" rot="R270" />
              <text x="-2.032" y="1.778" size="1.778" layer="95" rot="MR180">C</text>
              <text x="-2.032" y="7.62" size="1.778" layer="95" rot="MR180">A</text>
              <text x="-5.08" y="0" size="1.778" layer="95" rot="R90">&gt;NAME</text>
              <text x="-2.54" y="0" size="1.778" layer="96" rot="R90">&gt;VALUE</text>
              <polygon layer="94" width="0.254">
                <vertex x="0" y="2.54" />
                <vertex x="-1.27" y="5.08" />
                <vertex x="1.27" y="5.08" />
              </polygon>
              <polygon layer="94" width="0.254">
                <vertex x="3.556" y="6.35" />
                <vertex x="3.048" y="6.096" />
                <vertex x="3.302" y="5.842" />
              </polygon>
              <polygon layer="94" width="0.254">
                <vertex x="3.302" y="5.08" />
                <vertex x="2.794" y="4.826" />
                <vertex x="3.048" y="4.572" />
              </polygon>
            </symbol>
          </symbols>
          <devicesets>
            <deviceset prefix="D" name="BAT54">
              <description />
              <gates>
                <gate name="G$1" symbol="LED_WITH_PIN1_CATHODE" x="0" y="-5.08" />
              </gates>
              <devices>
                <device package="BAT54">
                  <connects>
                    <connect gate="G$1" pin="A" pad="1" />
                    <connect gate="G$1" pin="C" pad="3" />
                  </connects>
                  <technologies>
                    <technology name="" />
                  </technologies>
                </device>
              </devices>
            </deviceset>
          </devicesets>
        </library>
        <library name="adafruit">
          <description />
          <packages>
            <package name="FIDUCIAL_1MM">
              <description />
              <smd name="1" x="0" y="0" dx="1" dy="1" layer="1" roundness="100" stop="no" cream="no" />
              <polygon layer="29" width="0.127">
                <vertex x="-1" y="0" curve="90" />
                <vertex x="0" y="-1" curve="90" />
                <vertex x="1" y="0" curve="90" />
                <vertex x="0" y="1" curve="90" />
              </polygon>
              <polygon layer="41" width="0.127">
                <vertex x="-1" y="0" curve="90" />
                <vertex x="0" y="-1" curve="90" />
                <vertex x="1" y="0" curve="90" />
                <vertex x="0" y="1" curve="90" />
              </polygon>
              <polygon layer="39" width="0.127">
                <vertex x="-1" y="0" curve="90" />
                <vertex x="0" y="-1" curve="90" />
                <vertex x="1" y="0" curve="90" />
                <vertex x="0" y="1" curve="90" />
              </polygon>
            </package>
          </packages>
          <symbols />
          <devicesets>
            <deviceset name="FIDUCIAL">
              <description />
              <gates />
              <devices>
                <device package="FIDUCIAL_1MM" name="1MM">
                  <connects />
                  <technologies>
                    <technology name="" />
                  </technologies>
                </device>
              </devices>
            </deviceset>
          </devicesets>
        </library>
      </libraries>
      <attributes />
      <variantdefs />
      <classes>
        <class number="0" name="default" />
      </classes>
      <parts>
        <part device="" value="CL21B473KBC5PNC" name="CT" library="mlcc" deviceset="C_0805" />
        <part device="" value="LM555CMM/NOPB" name="LM555D" library="ecad" deviceset="LM555CMM/NOPB" />
        <part device="" value="ERJ-3EKF2322V" name="RA" library="resistor" deviceset="RESISTOR_0603" />
        <part device="" value="ERJ-3EKF2322V" name="RB" library="resistor" deviceset="RESISTOR_0603" />
        <part device="" value="08053D106KAT2A" name="C_BULK" library="mlcc" deviceset="C_0805" />
        <part device="" value="BS6I" name="J1_9V" library="General_Will" deviceset="BS6I" />
        <part device="" value="APT1608SGC" name="LED_G1" library="ecad" deviceset="LED-0603-KINGBRIGHT" />
        <part device="" value="RNCP0603FTD1K00" name="R_1K" library="resistor" deviceset="RESISTOR_0603" />
        <part device="" value="TL3342F160QG" name="SW_TOG" library="TL3342F160QG-697586" deviceset="TL3342F160QG" />
        <part device="PTH-KIT" value="CEM-1203(42)" name="BZZ" library="SparkFun-Electromechanical" deviceset="BUZZER" />
        <part device="" value="08053D106KAT2A" name="C_OUT" library="mlcc" deviceset="C_0805" />
        <part device="" value="BAT54" name="D1" library="magic_man" deviceset="BAT54" />
        <part device="" value="BAT54" name="D2" library="magic_man" deviceset="BAT54" />
        <part device="1MM" value="" name="FID1" library="adafruit" deviceset="FIDUCIAL" />
        <part device="1MM" value="" name="FID2" library="adafruit" deviceset="FIDUCIAL" />
        <part device="1MM" value="" name="FID3" library="adafruit" deviceset="FIDUCIAL" />
        <part device="1MM" value="" name="FID4" library="adafruit" deviceset="FIDUCIAL" />
        <part device="1MM" value="" name="FID5" library="adafruit" deviceset="FIDUCIAL" />
        <part device="1MM" value="" name="FID6" library="adafruit" deviceset="FIDUCIAL" />
      </parts>
      <sheets>
        <sheet>
          <description />
          <plain />
          <instances>
            <instance y="93.98" part="CT" gate="G$1" x="129.54" />
            <instance y="71.12" part="LM555D" gate="G$1" x="124.46" />
            <instance y="48.26" part="RA" gate="G$1" x="99.06" />
            <instance y="88.90" part="RB" gate="G$1" x="116.84" />
            <instance y="88.90" part="C_BULK" gate="G$1" x="33.02" />
            <instance y="33.02" part="J1_9V" gate="G$1" x="15.24" />
            <instance y="96.52" part="LED_G1" gate="G$1" x="50.80" />
            <instance y="83.82" part="R_1K" gate="G$1" x="45.72" />
            <instance y="60.96" part="SW_TOG" gate="G$1" x="45.72" />
            <instance y="109.22" part="BZZ" gate="G$1" x="93.98" />
            <instance y="111.76" part="C_OUT" gate="G$1" x="81.28" />
            <instance y="43.18" part="D1" gate="G$1" x="71.12" />
            <instance y="25.40" part="D2" gate="G$1" x="71.12" />
          </instances>
          <busses />
          <nets>
            <net name="N$0">
              <segment>
                <wire x1="129.54" y1="93.98" x2="129.54" y2="96.52" width="0.3" layer="91" />
                <label x="129.54" y="96.52" size="1.27" layer="95" />
                <pinref part="CT" gate="G$1" pin="P$1" />
              </segment>
              <segment>
                <wire x1="121.92" y1="88.90" x2="124.46" y2="88.90" width="0.3" layer="91" />
                <label x="124.46" y="88.90" size="1.27" layer="95" />
                <pinref part="RB" gate="G$1" pin="2" />
              </segment>
              <segment>
                <wire x1="147.32" y1="68.58" x2="149.86" y2="68.58" width="0.3" layer="91" />
                <label x="149.86" y="68.58" size="1.27" layer="95" />
                <pinref part="LM555D" gate="G$1" pin="THRESHOLD" />
              </segment>
              <segment>
                <wire x1="101.60" y1="73.66" x2="99.06" y2="73.66" width="0.3" layer="91" />
                <label x="99.06" y="73.66" size="1.27" layer="95" />
                <pinref part="LM555D" gate="G$1" pin="TRIGGER" />
              </segment>
            </net>
            <net name="N$1">
              <segment>
                <wire x1="129.54" y1="86.36" x2="129.54" y2="83.82" width="0.3" layer="91" />
                <label x="129.54" y="83.82" size="1.27" layer="95" />
                <pinref part="CT" gate="G$1" pin="P$2" />
              </segment>
              <segment>
                <wire x1="71.12" y1="33.02" x2="71.12" y2="35.56" width="0.3" layer="91" />
                <label x="71.12" y="35.56" size="1.27" layer="95" />
                <pinref part="D2" gate="G$1" pin="A" />
              </segment>
              <segment>
                <wire x1="17.78" y1="38.10" x2="17.78" y2="40.64" width="0.3" layer="91" />
                <label x="17.78" y="40.64" size="1.27" layer="95" />
                <pinref part="J1_9V" gate="G$1" pin="NEG" />
              </segment>
              <segment>
                <wire x1="50.80" y1="96.52" x2="50.80" y2="93.98" width="0.3" layer="91" />
                <label x="50.80" y="93.98" size="1.27" layer="95" />
                <pinref part="LED_G1" gate="G$1" pin="C" />
              </segment>
              <segment>
                <wire x1="33.02" y1="81.28" x2="33.02" y2="78.74" width="0.3" layer="91" />
                <label x="33.02" y="78.74" size="1.27" layer="95" />
                <pinref part="C_BULK" gate="G$1" pin="P$2" />
              </segment>
              <segment>
                <wire x1="101.60" y1="78.74" x2="99.06" y2="78.74" width="0.3" layer="91" />
                <label x="99.06" y="78.74" size="1.27" layer="95" />
                <pinref part="LM555D" gate="G$1" pin="GND" />
              </segment>
            </net>
            <net name="N$2">
              <segment>
                <wire x1="101.60" y1="63.50" x2="99.06" y2="63.50" width="0.3" layer="91" />
                <label x="99.06" y="63.50" size="1.27" layer="95" />
                <pinref part="LM555D" gate="G$1" pin="RESET" />
              </segment>
              <segment>
                <wire x1="93.98" y1="106.68" x2="93.98" y2="104.14" width="0.3" layer="91" />
                <label x="93.98" y="104.14" size="1.27" layer="95" />
                <pinref part="BZZ" gate="G$1" pin="1" />
              </segment>
              <segment>
                <wire x1="71.12" y1="43.18" x2="71.12" y2="40.64" width="0.3" layer="91" />
                <label x="71.12" y="40.64" size="1.27" layer="95" />
                <pinref part="D1" gate="G$1" pin="C" />
              </segment>
              <segment>
                <wire x1="58.42" y1="71.12" x2="58.42" y2="73.66" width="0.3" layer="91" />
                <label x="58.42" y="73.66" size="1.27" layer="95" />
                <pinref part="SW_TOG" gate="G$1" pin="4" />
              </segment>
              <segment>
                <wire x1="40.64" y1="83.82" x2="38.10" y2="83.82" width="0.3" layer="91" />
                <label x="38.10" y="83.82" size="1.27" layer="95" />
                <pinref part="R_1K" gate="G$1" pin="1" />
              </segment>
              <segment>
                <wire x1="33.02" y1="88.90" x2="33.02" y2="91.44" width="0.3" layer="91" />
                <label x="33.02" y="91.44" size="1.27" layer="95" />
                <pinref part="C_BULK" gate="G$1" pin="P$1" />
              </segment>
              <segment>
                <wire x1="33.02" y1="71.12" x2="33.02" y2="73.66" width="0.3" layer="91" />
                <label x="33.02" y="73.66" size="1.27" layer="95" />
                <pinref part="SW_TOG" gate="G$1" pin="3" />
              </segment>
              <segment>
                <wire x1="93.98" y1="48.26" x2="91.44" y2="48.26" width="0.3" layer="91" />
                <label x="91.44" y="48.26" size="1.27" layer="95" />
                <pinref part="RA" gate="G$1" pin="1" />
              </segment>
              <segment>
                <wire x1="147.32" y1="78.74" x2="149.86" y2="78.74" width="0.3" layer="91" />
                <label x="149.86" y="78.74" size="1.27" layer="95" />
                <pinref part="LM555D" gate="G$1" pin="VCC" />
              </segment>
            </net>
            <net name="N$3">
              <segment>
                <wire x1="101.60" y1="68.58" x2="99.06" y2="68.58" width="0.3" layer="91" />
                <label x="99.06" y="68.58" size="1.27" layer="95" />
                <pinref part="LM555D" gate="G$1" pin="OUTPUT" />
              </segment>
              <segment>
                <wire x1="71.12" y1="50.80" x2="71.12" y2="53.34" width="0.3" layer="91" />
                <label x="71.12" y="53.34" size="1.27" layer="95" />
                <pinref part="D1" gate="G$1" pin="A" />
              </segment>
              <segment>
                <wire x1="71.12" y1="25.40" x2="71.12" y2="22.86" width="0.3" layer="91" />
                <label x="71.12" y="22.86" size="1.27" layer="95" />
                <pinref part="D2" gate="G$1" pin="C" />
              </segment>
              <segment>
                <wire x1="81.28" y1="111.76" x2="81.28" y2="114.30" width="0.3" layer="91" />
                <label x="81.28" y="114.30" size="1.27" layer="95" />
                <pinref part="C_OUT" gate="G$1" pin="P$1" />
              </segment>
            </net>
            <net name="N$4">
              <segment>
                <wire x1="147.32" y1="73.66" x2="149.86" y2="73.66" width="0.3" layer="91" />
                <label x="149.86" y="73.66" size="1.27" layer="95" />
                <pinref part="LM555D" gate="G$1" pin="DISCHARGE" />
              </segment>
              <segment>
                <wire x1="111.76" y1="88.90" x2="109.22" y2="88.90" width="0.3" layer="91" />
                <label x="109.22" y="88.90" size="1.27" layer="95" />
                <pinref part="RB" gate="G$1" pin="1" />
              </segment>
              <segment>
                <wire x1="104.14" y1="48.26" x2="106.68" y2="48.26" width="0.3" layer="91" />
                <label x="106.68" y="48.26" size="1.27" layer="95" />
                <pinref part="RA" gate="G$1" pin="2" />
              </segment>
            </net>
            <net name="N$5">
              <segment>
                <wire x1="12.70" y1="38.10" x2="12.70" y2="40.64" width="0.3" layer="91" />
                <label x="12.70" y="40.64" size="1.27" layer="95" />
                <pinref part="J1_9V" gate="G$1" pin="POS" />
              </segment>
              <segment>
                <wire x1="33.02" y1="33.02" x2="33.02" y2="30.48" width="0.3" layer="91" />
                <label x="33.02" y="30.48" size="1.27" layer="95" />
                <pinref part="SW_TOG" gate="G$1" pin="1" />
              </segment>
              <segment>
                <wire x1="58.42" y1="33.02" x2="58.42" y2="30.48" width="0.3" layer="91" />
                <label x="58.42" y="30.48" size="1.27" layer="95" />
                <pinref part="SW_TOG" gate="G$1" pin="2" />
              </segment>
            </net>
            <net name="N$6">
              <segment>
                <wire x1="50.80" y1="104.14" x2="50.80" y2="106.68" width="0.3" layer="91" />
                <label x="50.80" y="106.68" size="1.27" layer="95" />
                <pinref part="LED_G1" gate="G$1" pin="A" />
              </segment>
              <segment>
                <wire x1="50.80" y1="83.82" x2="53.34" y2="83.82" width="0.3" layer="91" />
                <label x="53.34" y="83.82" size="1.27" layer="95" />
                <pinref part="R_1K" gate="G$1" pin="2" />
              </segment>
            </net>
            <net name="N$7">
              <segment>
                <wire x1="96.52" y1="106.68" x2="96.52" y2="104.14" width="0.3" layer="91" />
                <label x="96.52" y="104.14" size="1.27" layer="95" />
                <pinref part="BZZ" gate="G$1" pin="2" />
              </segment>
              <segment>
                <wire x1="81.28" y1="104.14" x2="81.28" y2="101.60" width="0.3" layer="91" />
                <label x="81.28" y="101.60" size="1.27" layer="95" />
                <pinref part="C_OUT" gate="G$1" pin="P$2" />
              </segment>
            </net>
          </nets>
        </sheet>
      </sheets>
      <errors />
    </schematic>
  </drawing>
  <compatibility />
</eagle>
