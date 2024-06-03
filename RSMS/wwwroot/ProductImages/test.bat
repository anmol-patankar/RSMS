@echo off

rem Define the list of new file names
set "newNames=toothpaste shampoo soap tissue water dishwashing sanitizer toiletpaper trashbags bleach cereal milk bread eggs bananas apples chicken beef pasta rice tomatoes carrots potatoes onions lettuce"

rem Set the directory containing the files
set "directory=C:\Users\coditas\Desktop\RSMS\RSMS\wwwroot\ProductImages"

rem Initialize the index for the new names
set /a "index=0"

rem Iterate over each .jpg file in the directory
for %%F in ("%directory%\*.jpg") do (
    rem Increment the index
    set /a "index+=1"
    
    rem Get the new file name from the list
    for /f "tokens=%index%" %%N in ("%newNames%") do (
        rem Rename the file
        ren "%%F" "%%N.jpg"
        echo Renamed "%%~nxF" to "%%N.jpg"
        exit /b
    )
)

echo All .jpg files renamed.
