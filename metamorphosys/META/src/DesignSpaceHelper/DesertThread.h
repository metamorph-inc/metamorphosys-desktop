// Copyright (C) 2013-2015 MetaMorph Software, Inc

// Permission is hereby granted, free of charge, to any person obtaining a
// copy of this data, including any software or models in source or binary
// form, as well as any drawings, specifications, and documentation
// (collectively "the Data"), to deal in the Data without restriction,
// including without limitation the rights to use, copy, modify, merge,
// publish, distribute, sublicense, and/or sell copies of the Data, and to
// permit persons to whom the Data is furnished to do so, subject to the
// following conditions:

// The above copyright notice and this permission notice shall be included
// in all copies or substantial portions of the Data.

// THE DATA IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
// THE AUTHORS, SPONSORS, DEVELOPERS, CONTRIBUTORS, OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE DATA OR THE USE OR OTHER DEALINGS IN THE DATA.  

// =======================
// This version of the META tools is a fork of an original version produced
// by Vanderbilt University's Institute for Software Integrated Systems (ISIS).
// Their license statement:

// Copyright (C) 2011-2014 Vanderbilt University

// Developed with the sponsorship of the Defense Advanced Research Projects
// Agency (DARPA) and delivered to the U.S. Government with Unlimited Rights
// as defined in DFARS 252.227-7013.

// Permission is hereby granted, free of charge, to any person obtaining a
// copy of this data, including any software or models in source or binary
// form, as well as any drawings, specifications, and documentation
// (collectively "the Data"), to deal in the Data without restriction,
// including without limitation the rights to use, copy, modify, merge,
// publish, distribute, sublicense, and/or sell copies of the Data, and to
// permit persons to whom the Data is furnished to do so, subject to the
// following conditions:

// The above copyright notice and this permission notice shall be included
// in all copies or substantial portions of the Data.

// THE DATA IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
// THE AUTHORS, SPONSORS, DEVELOPERS, CONTRIBUTORS, OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE DATA OR THE USE OR OTHER DEALINGS IN THE DATA.  

#ifndef DESERTTHREAD_H
#define DESERTTHREAD_H

#include <string>
#include "DesertStatusDlg.h"

typedef unsigned int UINT;

class CDesertStatusDlg;

class BaseNotify {
public:
  volatile bool m_quit;
  volatile bool m_cancel;
  volatile bool m_fail;
  CString m_invalidConstraint;
  UINT m_maxPrg;
  
public:

  BaseNotify(UINT maxPrg) : 
      m_maxPrg(maxPrg),
      m_quit(false),
	  m_cancel(false),
	  m_fail(false){}

	virtual void finished() { }

	virtual void reportStatus(StatusID s_id) { }
	virtual void reportProgress(const std::string &progress) { }
	virtual bool quit()
	{
		return m_quit;
	}
};

class Notify : public BaseNotify {
public:
  CDesertStatusDlg& m_dlg;
  CString m_invalidConstraint;
  UINT m_maxPrg;
  
public:

  Notify(CDesertStatusDlg& dlg, UINT maxPrg ) : 
	  BaseNotify(maxPrg),
      m_dlg(dlg)
	{}

  void finished();

  void reportStatus(StatusID s_id); 
  void reportProgress(const std::string &progress); 
};

//===================
class DesertThread
{
public: 
	/*DesertThread();*/
	DesertThread(DesertIface::DesertSystem &dsystem, 
		         const std::string &constraints, 
				 UdmDesertMap &des_map,
				 DesertUdmMap &inv_des_map,
				 BaseNotify* notify, 
				 int stage, long& configCount);
	DesertThread(DesertIfaceBack::DesertBackSystem &dbacksystem, 
		         UdmDesertMap &des_map,
				 DesertUdmMap &inv_des_map,
				 BaseNotify* notify, 
				 int stage, long& configCount);

  void operator()();
  void DesertThread::RunDesert();
private:
	DesertIface::DesertSystem m_ds;
	DesertIfaceBack::DesertBackSystem m_dbs;
	std::string m_constraints;
	BaseNotify*  m_notify;
	int m_stage;
	UdmDesertMap &m_des_map;
	DesertUdmMap &m_inv_des_map;
	long& m_configCount;
};

#endif