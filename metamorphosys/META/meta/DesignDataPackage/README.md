# HOWTO: make changes to the DesignDataPackage

1. Open in **eclipse-modeling-kepler-SR2-win32-x86_64**
2. Make mods
3. Right-click > File > Save as image file > `meta\DesignDataPackage\doc\ClassDiagrams\same_filename.pdf`
4. avm.genmodel right-click > Reload, click click click click
5. avm.genmodel right-click > Export Model, XML Schema, click click click click
6. `cd META\meta\DesignDataPackage`
7. `cmd /c make.bat`
8. Diff tool, remove some stuff:
```
git diff -w --no-color . | git apply --cached --ignore-whitespace
git checkout -- .
git reset HEAD -- .
git apply fix_codegen.patch
```

TODO: gitattributes EOL on xsd in EMF_MetaModel
