#!/bin/bash

log() {
    echo "[post-install] $1"
    date
}

log "Starting post install on pid $$"

log "Update package database"
sudo apt-get update -y

log "Install required packages"
sudo apt-get install -y pkg-config zip g++ zlib1g-dev unzip python ipython

log "Install python packages"
sudo apt-get -y install python-numpy python-dev python-wheel python-mock python-matplotlib python-pip python-pandas python-virtualenv

log "Upgrading pip"
pip install --upgrade pip
pip install --upgrade virtualenv --user

log "Install tensorflow"
pip install ipywidgets numpy scipy matplotlib ipython jupyter pandas sympy nose --user 
pip install tensorflow-gpu pandas astropy pydl absl-py --user

log "Install Bazel"
wget 'https://github.com/bazelbuild/bazel/releases/download/0.13.0/bazel-0.13.0-installer-linux-x86_64.sh'
chmod +x bazel-0.13.0-installer-linux-x86_64.sh
./bazel-0.13.0-installer-linux-x86_64.sh --user
rm -rf ./bazel-0.13.0-installer-linux-x86_64.sh

log "Update bashrc"
echo 'export LD_LIBRARY_PATH=${LD_LIBRARY_PATH:+${LD_LIBRARY_PATH}:}/usr/local/cuda/extras/CUPTI/lib64' >> ~/.bashrc
echo 'export PATH="$PATH:$HOME/bin"' >> ~/.bashrc

log "Clone git repo"
git clone https://github.com/tensorflow/models.git
