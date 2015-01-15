import os
import os.path
import sys
import subprocess

_this_dir = os.path.dirname(os.path.abspath(__file__))

if __name__ == '__main__':
    test_dir = os.path.join(_this_dir, 'test')
    sys.path.insert(0, os.path.join(_this_dir, '..', 'META', 'test'))
    import run_tests_console_output_xml
    run_tests_console_output_xml.run_tests(os.path.join(_this_dir, 'test\\tests.xunit'), test_dir, test_dir)
else:
    raise ImportError('This file is deprecated; use the one in META\\test instead')
