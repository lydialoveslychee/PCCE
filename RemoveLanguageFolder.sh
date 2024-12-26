#!/bin/bash

# Check if a path is provided
if [ -z "$1" ]; then
  echo "Usage: $0 <path>"
  exit 1
fi

# Assign the first argument to a variable
TARGET_PATH=$1

# Check if the path exists
if [ ! -d "$TARGET_PATH" ] && [ ! -f "$TARGET_PATH" ]; then
  echo "Error: Path '$TARGET_PATH' does not exist."
  exit 1
fi


# Delete the specified path
rm -rf "$TARGET_PATH"

# Check if the deletion was successful
if [ $? -eq 0 ]; then
  echo "Successfully deleted '$TARGET_PATH'."
else
  echo "Error: Failed to delete '$TARGET_PATH'."
  exit 1
fi