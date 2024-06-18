@echo off
setlocal

rem Source file
set src=sampleimage.jpg

rem List of new names
set names=toothpaste shampoo soap tissue water dishwashing sanitizer toiletpaper trashbags bleach cereal milk bread eggs bananas apples chicken beef pasta rice tomatoes carrots potatoes onions lettuce

rem Loop through each name and copy the file
for %%i in (%names%) do (
    copy "%src%" "%%i.jpg"
)

endlocal
