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

(ns dom-fun-eg.core
  (:import [org.zeromq ZMQ ZMQ$Poller])
  (:require [clojure.edn :as edn])
  (:require [clojure.data.json :as json]) )



(def connection-name "tcp://127.0.0.1:15150")


(defn do-task [& value]
  (let [ctx (ZMQ/context 1)
        sock (.socket ctx ZMQ/DEALER)]
    (.connect sock connection-name)
    (.send sock "META" ZMQ/SNDMORE)
    (.send sock "0.0.2" ZMQ/SNDMORE)
    (doseq [item (butlast value)]
      (.send sock item ZMQ/SNDMORE) )
    (.send sock (last value))
    sock))


(defn echo [& message] (do-task (conj message "echo")))
(defn start-creo [] (do-task "start-creo" ))
(defn stop-creo [] (do-task "stop-creo" ))
(defn dump-all-features [] (do-task "dump-features" "all" ))


(defn dump-feature [featureName]
  (do-task "dump-feature" featureName))

(defn dump-selected-features []
  (do-task "dump-selected-feature"))

(defn dump-selected-feature-xml [filename]
  "prompt the user for a selection and then write
  it to the the file specified in the argument"
  (do-task "dump-selected-feature-xml" filename))

(defn create-shell [shell-name]
  "export the current assembly to the specified name (file)"
  (do-task "create-shell" shell-name))

(defn load-shell [shell-name]
  "export the current assembly to the specified file"
  (do-task "load-shell" shell-name))

(defn init-shell [name type]
  "set the working solid to the specified assembly (file)"
  (do-task "init-shell" name type))

(defn init-shell-current []
  "set the working solid to the current assembly"
  (do-task "init-shell"))

(defn activate-shell [shell-name]
  "export the current assembly to the specified file"
  (do-task "activate-shell" shell-name))

(defn disassemble [payload]
  "disassemble the current assembly"
  (do-task "disassemble" payload))


(defn get-hydrostatic-result  [tolerance depth heel trim]
  "This function obtains a result containing:
  - displaced volume : the volume of fluid displaced based on depth and angle.
  - wetted area : the area of the body in contact with the fluid."
  (do-task "get-hydrostatic-result" tolerance depth heel trim))


(defn get-hydrostatic-space []
  (do-task "get-hydrostatic-space"))

(defn set-displacement  [value]
  (do-task "set-displacement" value))

(defn set-trim  [value]
  (do-task "set-trim" value))

(defn set-heel  [value]
  (do-task "set-heel" value))

(defn denormalize-assembly []
  (do-task "denormalize-assembly"))

(defn infer-joint [joint-list]
  (apply do-task
         "infer-joint"
         (for [joint joint-list] (json/write-str joint) ) ) )

