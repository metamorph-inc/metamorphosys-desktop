; Copyright (C) 2013-2015 MetaMorph Software, Inc

; Permission is hereby granted, free of charge, to any person obtaining a
; copy of this data, including any software or models in source or binary
; form, as well as any drawings, specifications, and documentation
; (collectively "the Data"), to deal in the Data without restriction,
; including without limitation the rights to use, copy, modify, merge,
; publish, distribute, sublicense, and/or sell copies of the Data, and to
; permit persons to whom the Data is furnished to do so, subject to the
; following conditions:

; The above copyright notice and this permission notice shall be included
; in all copies or substantial portions of the Data.

; THE DATA IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
; IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
; FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
; THE AUTHORS, SPONSORS, DEVELOPERS, CONTRIBUTORS, OR COPYRIGHT HOLDERS BE
; LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
; OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
; WITH THE DATA OR THE USE OR OTHER DEALINGS IN THE DATA.  

; =======================
; This version of the META tools is a fork of an original version produced
; by Vanderbilt University's Institute for Software Integrated Systems (ISIS).
; Their license statement:

; Copyright (C) 2011-2014 Vanderbilt University

; Developed with the sponsorship of the Defense Advanced Research Projects
; Agency (DARPA) and delivered to the U.S. Government with Unlimited Rights
; as defined in DFARS 252.227-7013.

; Permission is hereby granted, free of charge, to any person obtaining a
; copy of this data, including any software or models in source or binary
; form, as well as any drawings, specifications, and documentation
; (collectively "the Data"), to deal in the Data without restriction,
; including without limitation the rights to use, copy, modify, merge,
; publish, distribute, sublicense, and/or sell copies of the Data, and to
; permit persons to whom the Data is furnished to do so, subject to the
; following conditions:

; The above copyright notice and this permission notice shall be included
; in all copies or substantial portions of the Data.

; THE DATA IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
; IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
; FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
; THE AUTHORS, SPONSORS, DEVELOPERS, CONTRIBUTORS, OR COPYRIGHT HOLDERS BE
; LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
; OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
; WITH THE DATA OR THE USE OR OTHER DEALINGS IN THE DATA.  

(require '[dom-fun-eg.core :as eg])


(eg/init-shell "twoplates" "asm")
;;  m_ex_shell.set_name( file_stem );
;; 	m_ex_shell.activate_model( file_stem,
;; 			(file_extension == "asm" ? PRO_MDL_ASSEMBLY : PRO_MDL_PART ) );


(eg/create-shell "whatsit")
;;   if (! m_ex_shell.has_base_solid()) {
;;			m_log_cf.warnStream() << "no current model ";
;;			return false;
;;		}
;; 	  switch( status = m_ex_shell.create_shrinkwrap(name) ) {
;;			case PRO_TK_NO_ERROR: break;
;;			default:
;;				m_log_cf.warnStream() << "something went wrong " << status;
;;	  }

(eg/get-hydrostatic-result "0.001" "1123456" "0.0" "0.0")
;;    if (! m_ex_shell.has_wrapped_solid()) {
;;			m_log_cf.warnStream() << "no current wrapped model ";
;;			return false;
;;		}
;;		m_ex_shell.set_current_solid_to_wrapped();
;;    isis::hydrostatic::Result result;
;;		m_ex_shell.computeHydrostatic(result, true, tolerance, displaced, heel, trim);


(eg/set-displacement "2000")
(eg/get-hydrostatic-space)
