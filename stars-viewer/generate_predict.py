
from __future__ import absolute_import
from __future__ import division
from __future__ import print_function

import argparse
import sys

import matplotlib.pyplot as plt
import numpy as np
import tensorflow as tf

import pandas as pd

from astronet import models
from astronet.data import preprocess
from astronet.util import config_util
from astronet.util import configdict
from astronet.util import estimator_util

import requests

_LABEL_COLUMN = "av_training_set"
_ALLOWED_LABELS = {"PC", "AFP", "NTP"}

_MODEL_DIR = "/home/vmadmin/astronet/model/"
_KEPLER_DATA_DIR = "/datadrive/kepler"
_TCE_CSV_FILE = "/home/vmadmin/astronet/dr24_tce.csv"

tce_table = pd.read_csv(_TCE_CSV_FILE, index_col="rowid", comment="#")
tce_table = tce_table.drop_duplicates(subset=["kepid"])

allowed_tces = tce_table[_LABEL_COLUMN].apply(lambda l: l in _ALLOWED_LABELS)

tce_table = tce_table[allowed_tces]


def _process_tce(feature_config, kepler_id, period, t0, duration, kepler_data):
    time, flux = preprocess.read_and_process_light_curve(
        kepler_id, kepler_data)
    time, flux = preprocess.phase_fold_and_sort_light_curve(
        time, flux, period, t0)

    features = {}

    if "global_view" in feature_config:
        global_view = preprocess.global_view(time, flux, period)
        features["global_view"] = np.expand_dims(global_view, 0)

    if "local_view" in feature_config:
        local_view = preprocess.local_view(time, flux, period, duration)
        features["local_view"] = np.expand_dims(local_view, 0)
    return features


def create_model_class(model_name="AstroCNNModel"):
    return models.get_model_class(model_name)


def create_config(model_name="AstroCNNModel", config_name="local_global"):
    config = (
        models.get_model_config(model_name, config_name)
        if config_name else config_util.parse_json("config_json"))
    return configdict.ConfigDict(config)


def create_estimator(model_dir, model_class, config):
    return estimator_util.create_estimator(model_class, config.hparams, model_dir=model_dir)


def get_predictions(kepler_data, kepler_id, period, t0, duration, config, estimator):
    features = _process_tce(config.inputs.features,
                            kepler_id, period, t0, duration, kepler_data)

    def input_fn():
        time_series_features = tf.estimator.inputs.numpy_input_fn(
            features, batch_size=1, shuffle=False, queue_capacity=1)()
        return {"time_series_features": time_series_features}
    return estimator.predict(input_fn)


def predict(kepler_data, kepler_id, period, t0, duration, config, estimator):
    for predictions in get_predictions(kepler_data, kepler_id, period, t0, duration, config, estimator):
        assert len(predictions) == 1
        return predictions[0]


def main():
    model_class = create_model_class()
    config = create_config()
    estimator = create_estimator(_MODEL_DIR, model_class, config)
    predictions = []
    for i, row in tce_table.iterrows():
        kepler_id = row['kepid']
        period = row["tce_period"]
        duration = row["tce_duration"]
        t0 = 2.2
        prediction = predict(_KEPLER_DATA_DIR, kepler_id, period,
                             t0, duration, config, estimator)
        predictions.append([kepler_id, prediction])
        print("Kepler ", kepler_id, " has prediction ", prediction)
        if i % 200 == 0:
            resp = requests.post(
                "http://localhost:3000/predictions/partition", json=predictions)
            if resp.status_code == 200:
                print("Part of predictions was sent successful")
            else:
                print("Part of predictions was not sent")
            predictions.clear()
    save_resp = requests.post("http://localhost:3000/predictions/save")
    if save_resp.status_code == 200:
        print("Predictions was saved successfull. You can watch results on http://localhost:3000/")
    else:
        print("Predictions was not saved. Please try again.")


main()
