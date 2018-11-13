import "babel-polyfill";

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

export const fetchAllToggles = () => async dispatch => {
  dispatch(allTogglesRequested());

  const response = await fetch(`/api/toggles`);
  const json = await response.json();
  dispatch(allTogglesReceived(json));
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

export const updateName = (toggleID, newName) => async dispatch => {
  dispatch(updateToggleNameRequested(toggleID, newName));

  const response = await fetch(`/api/toggles/id/${toggleID}/name`, {
    method: "PUT",
    headers: {
      "Content-Type": "application/json"
    },
    body: JSON.stringify({ name: newName })
  });

  const json = await response.json();

  dispatch(toggleNameUpdated(json.toggleID, json.name));
};
