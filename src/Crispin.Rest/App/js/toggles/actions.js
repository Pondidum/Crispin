export const ALL_TOGGLES_REQUESTED = "ALL_TOGGLES_REQUESTED";
export const ALL_TOGGLES_RECEIVED = "ALL_TOGGLES_RECEIVED";

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
