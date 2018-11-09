export const ALL_TOGGLES_REQUESTED = "ALL_TOGGLES_REQUESTED";
export const ALL_TOGGLES_RECEIVED = "ALL_TOGGLES_RECEIVED";
export const UPDATE_TOGGLE_NAME_REQUESTED = "UPDATE_TOGGLE_NAME_REQUESTED";
export const UPDATE_TOGGLE_NAME_COMPLETED = "UPDATE_TOGGLE_NAME_COMPLETED";

export const allTogglesRequested = () => ({
  type: ALL_TOGGLES_REQUESTED
});

export const allTogglesReceived = toggles => ({
  type: ALL_TOGGLES_RECEIVED,
  toggles
});

export const fetchAllToggles = () => dispatch => {
  dispatch(allTogglesRequested());
  return fetch(`/api/toggles`)
    .then(response => response.json())
    .then(json => dispatch(allTogglesReceived(json)));
};

export const updateToggleNameRequested = (toggleID, newName) => ({
  type: UPDATE_TOGGLE_NAME_REQUESTED,
  toggleID,
  newName
});

export const toggleNameUpdated = (toggleID, newName) => ({
  type: UPDATE_TOGGLE_NAME_COMPLETED,
  toggleID,
  newName
});

export const updateName = (toggleID, newName) => dispatch => {
  dispatch(updateToggleNameRequested(toggleID, newName));
  return fetch({ method: "PUT", url: `/api/toggles` })
    .then(response => response.json())
    .then(json => dispatch(toggleNameUpdated(json.id, json.name)));
};
